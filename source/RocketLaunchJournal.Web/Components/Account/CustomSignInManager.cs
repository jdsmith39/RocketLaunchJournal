using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RocketLaunchJournal.Model.UserIdentity;
using RocketLaunchJournal.Web.Shared.UserIdentity;
using System.Security.Claims;

namespace RocketLaunchJournal.Web.Components.Account;

public class CustomSignInManager : SignInManager<User>
{
  public CustomSignInManager(UserManager<User> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<User>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<User> confirmation)
    : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
  {
  }

  public override async Task SignInWithClaimsAsync(User user, AuthenticationProperties? authenticationProperties, IEnumerable<Claim> additionalClaims)
  {
    var userPrincipal = await CreateUserPrincipalAsync(user);
    foreach (var claim in additionalClaims)
    {
      userPrincipal.Identities.First().AddClaim(claim);
    }

    var roles = user.UserRoles?.Select(s => s.Role).ToList();
    foreach (var claim in UserClaimBuilder.GenerateClaimsServer(user, roles, null, null))
    {
      userPrincipal.Identities.First().AddClaim(claim);
    }

    await Context.SignInAsync(AuthenticationScheme,
        userPrincipal,
        authenticationProperties ?? new AuthenticationProperties());

    // This is useful for updating claims immediately when hitting MapIdentityApi's /account/info endpoint with cookies.
    Context.User = userPrincipal;
  }
}
