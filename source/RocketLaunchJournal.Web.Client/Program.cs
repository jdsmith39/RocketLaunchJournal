using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RocketLaunchJournal.Web.Client.Helpers;
using RocketLaunchJournal.Web.Client.Services;
using RocketLaunchJournal.Web.Shared.UserIdentity.Policies;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient("RocketLaunchJournal.Web.ServerAPI.Anonymous", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient(RocketLaunchJournal.Web.Shared.Constants.Identity.Scope, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("RocketLaunchJournal.Web.ServerAPI.Anonymous"));
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(RocketLaunchJournal.Web.Shared.Constants.Identity.Scope));

            builder.Services.AddTransient<AnonymousClient>((sp) => new AnonymousClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RocketLaunchJournal.Web.ServerAPI.Anonymous"), sp.GetRequiredService<ServiceResponseHandler>()));
            builder.Services.AddTransient<AuthorizedClient>((sp) => new AuthorizedClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient(RocketLaunchJournal.Web.Shared.Constants.Identity.Scope), sp.GetRequiredService<ServiceResponseHandler>()));

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

            builder.Services.AddBlazoredToast();
            builder.Services.AddBlazoredModal();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            builder.Services.AddScoped<ServiceResponseHandler>();
            
            await builder.Build().RunAsync();
        }
    }
}
