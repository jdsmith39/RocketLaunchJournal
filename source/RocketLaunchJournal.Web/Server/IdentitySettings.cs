using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using RocketLaunchJournal.Web.Shared.UserIdentity;
using System.Collections.Generic;

namespace RocketLaunchJournal.Web.Server;

public static class IdentitySettings
{
    public static IList<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new ApiScope(name: Shared.Constants.Identity.Scope,   displayName: Shared.Constants.Identity.Scope),
        };
    }

    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            //new IdentityResource()
            //{
            //    Name = "rocketLaunchJournal.web.api",
            //    UserClaims =
            //    {
            //        JwtClaimTypes.GivenName,
            //        JwtClaimTypes.FamilyName,
            //        UserClaimBuilder.RoleDataClaimType,
            //        UserClaimBuilder.IPAddressClaimType,
            //        JwtClaimTypes.Subject + UserClaimBuilder.OriginalSuffix
            //    }
            //}
        };

    public static IEnumerable<ApiResource> GetApis() =>
        new List<ApiResource> {
            new ApiResource(Shared.Constants.Identity.Scope, new[] {
                JwtClaimTypes.Subject,
                JwtClaimTypes.Role,
                JwtClaimTypes.GivenName,
                JwtClaimTypes.FamilyName,
                UserClaimBuilder.RoleDataClaimType,
                UserClaimBuilder.IPAddressClaimType,
                JwtClaimTypes.Subject + UserClaimBuilder.OriginalSuffix })
            {
                Scopes = { Shared.Constants.Identity.Scope }
            }
        };

    public static IEnumerable<Duende.IdentityServer.Models.Client> GetClients() =>
        new List<Duende.IdentityServer.Models.Client> {
            new Duende.IdentityServer.Models.Client {
                ClientName = "RocketLaunchJournal.Web.Client",
                ClientId = "RocketLaunchJournal.Web.Client",
                ClientSecrets = {  },
                RequireClientSecret = false,
                RequireConsent = false,
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "/authentication/login-callback" },
                PostLogoutRedirectUris = { "/authentication/logout-callback" },
                //AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    Shared.Constants.Identity.Scope },
                Properties = new Dictionary<string,string>()
                {
                    {"Profile", "IdentityServerSPA" }
                }
            },
        };
}

