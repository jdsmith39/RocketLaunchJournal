using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.Services.Users;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.UserIdentity;
using RocketLaunchJournal.Web.Client.Helpers;
using RocketLaunchJournal.Web.Client.Services;
using RocketLaunchJournal.Web.Components;
using RocketLaunchJournal.Web.Components.Account;
using RocketLaunchJournal.Web.Shared.UserIdentity;
using RocketLaunchJournal.Web.Shared.UserIdentity.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers().AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new TextJsonSerializer.DBNullConverter());
});

builder.Services.AddLocalization();
builder.Services.AddOptions();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddIdentityCore<User>(options =>
{
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 10, 0);
    options.Lockout.MaxFailedAccessAttempts = 3;

    options.Password.RequireDigit = false;
    options.Password.RequiredLength = RocketLaunchJournal.Model.Constants.FieldSizes.PasswordMinimumLength;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;

    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = true;
}).AddRoles<Role>().AddUserStore<RocketLaunchJournal.Entities.UserIdentity.UserStore>().AddRoleStore<RocketLaunchJournal.Entities.UserIdentity.RoleStore>()
.AddSignInManager<CustomSignInManager>().AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(PolicyNames.CanImpersonate, policy => policy.Requirements.Add(new CanImpersonateRequirement()));
    config.AddPolicy(PolicyNames.LaunchAddEditDelete, policy => policy.Requirements.Add(new LaunchAddEditDeleteRequirement()));
    config.AddPolicy(PolicyNames.ReportAddEditDelete, policy => policy.Requirements.Add(new ReportAddEditDeleteRequirement()));
    config.AddPolicy(PolicyNames.RocketAddEditDelete, policy => policy.Requirements.Add(new RocketAddEditDeleteRequirement()));
    config.AddPolicy(PolicyNames.UserAddEditDelete, policy => policy.Requirements.Add(new UserAddEditDeleteRequirement()));
    config.AddPolicy(PolicyNames.UserProfileEdit, policy => policy.Requirements.Add(new UserProfileEditRequirement()));
});
builder.Services.AddScoped<IAuthorizationHandler, CanImpersonate>();
builder.Services.AddScoped<IAuthorizationHandler, LaunchAddEditDelete>();
builder.Services.AddScoped<IAuthorizationHandler, ReportAddEditDelete>();
builder.Services.AddScoped<IAuthorizationHandler, RocketAddEditDelete>();
builder.Services.AddScoped<IAuthorizationHandler, UserAddEditDelete>();
builder.Services.AddScoped<IAuthorizationHandler, UserProfileEdit>();

builder.Services.AddScoped<IEmailSender<User>, CustomEmailSender>();

builder.Services.Configure<Services.Email.MailSettings>(builder.Configuration.GetSection(nameof(Services.Email.MailSettings)));
builder.Services.AddScoped<Services.Email.IEmailer, Services.Email.Emailer>();

builder.Services.AddScoped<UserPermissionService>((s) =>
{
    var httpContext = s.GetService<IHttpContextAccessor>();
    string? ipAddress = null;
    if (httpContext?.HttpContext?.Connection?.RemoteIpAddress != null)
    {
        if (httpContext.HttpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6)
            ipAddress = httpContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        else
            ipAddress = httpContext.HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();
    }
    var ups = new UserPermissionService(ipAddress);
    ups.Setup(new UserClaimBuilder(httpContext!.HttpContext?.User));
    return ups;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ILoggingContext, DataContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Templates.Emails.EmailsCreate>();
builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Launches.LaunchesCreateUpdate>();
builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Launches.LaunchesGet>();
builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Logs.APILogsCreateUpdate>();
builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Logs.SystemLogsCreate>();
builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Rockets.RocketsCreateUpdate>();
builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Rockets.RocketsGet>();
builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Adhoc.AdhocCreateUpdate>();
builder.Services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Adhoc.AdhocGet>();
builder.Services.AddTransient<UsersCreateUpdate>();
builder.Services.AddTransient<UsersGet>();

// Supply HttpClient instances that include access tokens when making requests to the server project
var url = builder.Configuration.GetValue<string>("ApiUrl") ?? throw new InvalidOperationException("Missing API Url");
builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(url) });

builder.Services.AddTransient<AnonymousClient>();
builder.Services.AddTransient<AuthorizedClient>();

builder.Services.AddScoped<ServiceResponseHandler>();

builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredModal();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(RocketLaunchJournal.Web.Client.Pages.Index).Assembly);

app.MapControllers();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
