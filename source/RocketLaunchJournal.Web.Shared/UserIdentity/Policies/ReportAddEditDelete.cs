using Microsoft.AspNetCore.Authorization;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Shared.UserIdentity.Policies
{
    public class ReportAddEditDelete : AuthorizationHandler<ReportAddEditDelete>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ReportAddEditDelete requirement)
        {
            var _userPermissionService = new UserClaimBuilder(context.User);
            if (_userPermissionService.UserPolicies!.ReportAddEditDelete)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
