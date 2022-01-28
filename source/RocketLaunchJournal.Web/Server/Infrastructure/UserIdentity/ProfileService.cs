using Duende.IdentityServer.Services;
using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RocketLaunchJournal.Infrastructure.Services.Users;
using RocketLaunchJournal.Model.UserIdentity;
using RocketLaunchJournal.Web.Shared.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Server.Infrastructure.UserIdentity
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IRoleStore<Role> _roleStore { get; }

        public ProfileService(UserManager<User> userManager, IUserClaimsPrincipalFactory<User> claimsFactory, IHttpContextAccessor httpContextAccessor, IRoleStore<Role> roleStore)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleStore = roleStore;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();
            if (context.Caller == IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint)
            {
                claims.AddRange(UserClaimBuilder.GenerateClaimsClient(user));
            }
            else
            {
                var roleNames = await _userManager.GetRolesAsync(user);
                var roles = new List<Role>();
                foreach (var item in roleNames)
                {
                    var role = await _roleStore.FindByNameAsync(item, default);
                    roles.Add(role);
                }

                string? ipAddress = null;
                if (_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6)
                    ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                else
                    ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();
                claims.AddRange(UserClaimBuilder.GenerateClaimsServer(user, roles, ipAddress));
            }
            
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null && !user.AuditFields!.InactiveDateTime.HasValue && user.IsLoginEnabled;
        }
    }
}
