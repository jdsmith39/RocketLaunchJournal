using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Shared.UserIdentity.Policies
{
    public class RocketAddEditDelete : AuthorizationHandler<RocketAddEditDelete>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RocketAddEditDelete requirement)
        {
            var userClaims = new UserClaimBuilder(context.User);
            if (userClaims.UserPolicies.RocketAddEditDelete)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
