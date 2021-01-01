using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Shared.UserIdentity.Policies
{
    public class UserProfileEdit : AuthorizationHandler<UserProfileEdit>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserProfileEdit requirement)
        {
            var userClaims = new UserClaimBuilder(context.User);
            if (userClaims.UserPolicies.UserProfileEdit)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
