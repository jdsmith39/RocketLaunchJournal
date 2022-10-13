using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.Services.Users;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.UserIdentity;
using RocketLaunchJournal.Web.Server.Infrastructure.UserIdentity;
using RocketLaunchJournal.Web.Shared.UserIdentity;
using RocketLaunchJournal.Web.Shared.UserIdentity.Policies;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace RocketLaunchJournal.Web.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IApplicationBuilder _app;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddOptions();

            services.AddLogging();

            services.AddHttpsRedirection(o =>
            {
                o.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                o.HttpsPort = 443;
            });

            // developer created class DI ***************************************
            DeveloperAddedServices(services);

            AuthenticationAndAuthorization(services);

            services.AddControllersWithViews().AddJsonOptions((options) =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new TextJsonSerializer.DBNullConverter());
            });
            services.AddRazorPages().AddJsonOptions((options) =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _app = app;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        /// <summary>
        /// All authentication and authorization setup goes here.
        /// </summary>
        private void AuthenticationAndAuthorization(IServiceCollection services)
        {
            services.AddDefaultIdentity<User>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 10, 0);
                options.Lockout.MaxFailedAccessAttempts = 3;

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = Model.Constants.FieldSizes.PasswordMinimumLength;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;
            }).AddRoles<Role>().AddUserStore<Entities.UserIdentity.UserStore>().AddRoleStore<Entities.UserIdentity.RoleStore>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddIdentityServer()
            .AddApiAuthorization<User, DataContext>((options) =>
            {
                options.ApiScopes = new ApiScopeCollection(IdentitySettings.GetApiScopes());
                options.Clients = new Microsoft.AspNetCore.ApiAuthorization.IdentityServer.ClientCollection(IdentitySettings.GetClients().ToList());
                options.ApiResources = new Microsoft.AspNetCore.ApiAuthorization.IdentityServer.ApiResourceCollection(IdentitySettings.GetApis().ToList());
                options.IdentityResources = new Microsoft.AspNetCore.ApiAuthorization.IdentityServer.IdentityResourceCollection(IdentitySettings.GetIdentityResources().ToList());
            })
            .AddProfileService<ProfileService>();

            services.AddAuthentication().AddIdentityServerJwt();

            // policies
            services.AddAuthorization(config =>
            {
                config.AddPolicy(PolicyNames.CanImpersonate, policy => policy.Requirements.Add(new CanImpersonate()));
                config.AddPolicy(PolicyNames.LaunchAddEditDelete, policy => policy.Requirements.Add(new LaunchAddEditDelete()));
                config.AddPolicy(PolicyNames.ReportAddEditDelete, policy => policy.Requirements.Add(new ReportAddEditDelete()));
                config.AddPolicy(PolicyNames.RocketAddEditDelete, policy => policy.Requirements.Add(new RocketAddEditDelete()));
                config.AddPolicy(PolicyNames.UserAddEditDelete, policy => policy.Requirements.Add(new UserAddEditDelete()));
                config.AddPolicy(PolicyNames.UserProfileEdit, policy => policy.Requirements.Add(new UserProfileEdit()));
            });
        }

        /// <summary>
        /// Services that the system uses that are added
        /// </summary>
        private void DeveloperAddedServices(IServiceCollection services)
        {
            services.Configure<Services.Email.MailSettings>(Configuration.GetSection(nameof(Services.Email.MailSettings)));
            services.AddTransient<Services.Email.IEmailer, Services.Email.Emailer>();

            services.AddScoped<UserPermissionService>((s) =>
            {
                var contextAccessor = s.GetService<IHttpContextAccessor>();
                var ups = new UserPermissionService();
                ups.Setup(new UserClaimBuilder(contextAccessor!.HttpContext?.User));
                return ups;
            });

            var dbConnection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(dbConnection));
            services.AddDbContext<ILoggingContext, DataContext>(options => options.UseSqlServer(dbConnection));

            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Templates.Emails.EmailsCreate>();
            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Launches.LaunchesCreateUpdate>();
            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Launches.LaunchesGet>();
            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Logs.APILogsCreateUpdate>();
            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Logs.SystemLogsCreate>();
            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Rockets.RocketsCreateUpdate>();
            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Rockets.RocketsGet>();
            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Adhoc.AdhocCreateUpdate>();
            services.AddTransient<RocketLaunchJournal.Infrastructure.Services.Adhoc.AdhocGet>();
            services.AddTransient<UsersCreateUpdate>();
            services.AddTransient<UsersGet>();
        }
    }
}
