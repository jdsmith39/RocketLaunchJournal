using Microsoft.AspNetCore.Authorization;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Shared.UserIdentity.Policies
{
    public class UserAddEditDelete : AuthorizationHandler<UserAddEditDelete>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserAddEditDelete requirement)
        {
            var userClaims = new UserClaimBuilder(context.User);
            if (userClaims!.UserPolicies!.UserAddEditDelete)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
