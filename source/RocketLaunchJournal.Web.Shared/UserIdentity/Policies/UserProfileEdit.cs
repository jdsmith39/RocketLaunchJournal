using Microsoft.AspNetCore.Authorization;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Shared.UserIdentity.Policies;

public class UserProfileEditRequirement : IAuthorizationRequirement
{
}

public class UserProfileEdit : AuthorizationHandler<UserProfileEditRequirement>
{
  private readonly UserPermissionService userPermissionService;

  public UserProfileEdit(UserPermissionService userPermissionService)
  {
    this.userPermissionService = userPermissionService;
  }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserProfileEditRequirement requirement)
  {
    if (!userPermissionService.IsSetup || (context.User.HasClaim(w => w.Type == ClaimTypes.Email) && userPermissionService.IsSetup && string.IsNullOrEmpty(userPermissionService.UserClaimModel.Email)))
      userPermissionService.Setup(new UserClaimBuilder(context.User));

    if (userPermissionService.UserPolicies?.UserProfileEdit ?? false)
    {
      context.Succeed(requirement);
    }

    return Task.CompletedTask;
  }
}