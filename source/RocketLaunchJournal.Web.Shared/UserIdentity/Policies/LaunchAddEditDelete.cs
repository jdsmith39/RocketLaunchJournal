using Microsoft.AspNetCore.Authorization;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Shared.UserIdentity.Policies
{
    public class LaunchAddEditDelete : AuthorizationHandler<LaunchAddEditDelete>, IAuthorizationRequirement
    {
        //private readonly UserPermissionService _userPermissionService;

        //public LaunchAddEditDelete(UserPermissionService ups)
        //{
        //    _userPermissionService = ups;
        //}

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LaunchAddEditDelete requirement)
        {
            var _userPermissionService = new UserClaimBuilder(context.User);
            if (_userPermissionService.UserPolicies!.LaunchAddEditDelete)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
