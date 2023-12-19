using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Web.Client.Helpers;
using RocketLaunchJournal.Web.Client.Services;
using RocketLaunchJournal.Web.Shared.UserIdentity.Policies;

namespace RocketLaunchJournal.Web.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddTransient<AnonymousClient>();
        builder.Services.AddTransient<AuthorizedClient>();

        builder.Services.AddApiAuthorization();

        builder.Services.AddAuthorizationCore(config =>
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

    builder.Services.AddScoped<UserPermissionService>((s) =>
        {
            var ups = new UserPermissionService(null);
            return ups;
        });

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

        builder.Services.AddBlazoredToast();
        builder.Services.AddBlazoredModal();

        builder.Services.AddScoped<ServiceResponseHandler>();

        await builder.Build().RunAsync();
    }
}
