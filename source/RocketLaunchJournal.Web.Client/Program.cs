using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
      config.AddPolicy(PolicyNames.CanImpersonate, policy => policy.Requirements.Add(new CanImpersonate()));
      config.AddPolicy(PolicyNames.LaunchAddEditDelete, policy => policy.Requirements.Add(new LaunchAddEditDelete()));
      config.AddPolicy(PolicyNames.ReportAddEditDelete, policy => policy.Requirements.Add(new ReportAddEditDelete()));
      config.AddPolicy(PolicyNames.RocketAddEditDelete, policy => policy.Requirements.Add(new RocketAddEditDelete()));
      config.AddPolicy(PolicyNames.UserAddEditDelete, policy => policy.Requirements.Add(new UserAddEditDelete()));
      config.AddPolicy(PolicyNames.UserProfileEdit, policy => policy.Requirements.Add(new UserProfileEdit()));
    });

    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

    builder.Services.AddBlazoredToast();
    builder.Services.AddBlazoredModal();

    builder.Services.AddScoped<ServiceResponseHandler>();

    await builder.Build().RunAsync();
  }
}