using RocketLaunchJournal.Model.SerializedObjects;

namespace RocketLaunchJournal.Infrastructure.UserIdentity;

public class UserClaimModel
{
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
