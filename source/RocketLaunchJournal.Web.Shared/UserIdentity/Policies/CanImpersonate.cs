using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Shared.UserIdentity.Policies
{
    public class CanImpersonate : AuthorizationHandler<CanImpersonate>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CanImpersonate requirement)
        {
            var userClaims = new UserClaimBuilder(context.User);
            if (userClaims.UserPolicies.CanImpersonate)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
