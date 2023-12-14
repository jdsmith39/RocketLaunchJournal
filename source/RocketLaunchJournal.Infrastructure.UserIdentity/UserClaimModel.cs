using RocketLaunchJournal.Model.SerializedObjects;
using RocketLaunchJournal.Model.UserIdentity;
using System.Collections.Generic;
using System.Security.Claims;

namespace RocketLaunchJournal.Infrastructure.UserIdentity;

public class UserClaimModel
{
  #region Claim Types

  public const string RoleDataClaimType = nameof(RoleData);
  public const string IPAddressClaimType = nameof(IpAddress);
  public const string OriginalSuffix = "Original";

  #endregion

  public bool IsAuthenticated { get { return UserId > 0; } }

  public int UserId { get; set; }
  public int UserIdOriginal { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Email { get; set; }

  public string? IpAddress { get; set; }

  public int UtcOffset { get; set; }

  // roles
  public bool IsAdmin { get; set; }

  public RoleData? RoleData { get; set; }

  // policies
  public UserPolicies? UserPolicies { get; set; }

  protected void SetupPolicies()
  {
    UserPolicies = new UserPolicies();
    UserPolicies.CanImpersonate = IsAdmin;
    UserPolicies.UserAddEditDelete = IsAdmin;
    UserPolicies.UserProfileEdit = IsAuthenticated;// everyone can

    // everyone can edit their own
    UserPolicies.RocketAddEditDelete = IsAuthenticated;
    // everyone can edit their own
    UserPolicies.LaunchAddEditDelete = IsAuthenticated;
    // everyone can edit their own
    UserPolicies.ReportAddEditDelete = IsAuthenticated;
  }

  /// <summary>
  /// Simplified claims for the client side.  
  /// </summary>
  /// <returns></returns>
  public List<Claim> GenerateClaimsFromUserClaimModel()
  {
    var claims = new List<Claim>()
    {
        new Claim(ClaimTypes.NameIdentifier, UserId.ToString(), ClaimValueTypes.Integer),
        new Claim(nameof(User.UserId) + OriginalSuffix, UserIdOriginal.ToString(), ClaimValueTypes.Integer),
        new Claim(ClaimTypes.GivenName, FirstName, ClaimValueTypes.String),
        new Claim(ClaimTypes.Surname, LastName, ClaimValueTypes.String),
        new Claim(ClaimTypes.Email, Email, ClaimValueTypes.String),
        new Claim(ClaimTypes.Name, Email, ClaimValueTypes.String),
        new Claim(ClaimTypes.Role, IsAdmin ? "Admin" : "", ClaimValueTypes.String)
    };

    return claims;
  }
}

public class UserPolicies
{
  public UserPolicies() { }
  public UserPolicies(UserPolicies userPolicies)
  {
    var properties = typeof(UserPolicies).GetProperties();
    foreach (var item in properties)
    {
      item.SetValue(this, item.GetValue(userPolicies));
    }
  }

  public bool RocketAddEditDelete { get; set; }
  public bool CanImpersonate { get; set; }
  public bool LaunchAddEditDelete { get; set; }
  public bool ReportAddEditDelete { get; set; }
  public bool UserAddEditDelete { get; set; }
  public bool UserProfileEdit { get; set; }
}
