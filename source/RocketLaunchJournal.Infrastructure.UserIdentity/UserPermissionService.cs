namespace RocketLaunchJournal.Infrastructure.UserIdentity;

public class UserPermissionService
{
  public UserPermissionService(string? ipAddress)
  {
    IpAddress = ipAddress;
  }

  public bool IsSetup { get; private set; }
  public string? IpAddress { get; private set; }
  public string? FirstLastName
  {
    get
    {
      if (UserClaimModel != null)
        return $"{UserClaimModel.FirstName} {UserClaimModel.LastName}";
      return null;
    }
  }

  public string? LastFirstName
  {
    get
    {
      if (UserClaimModel != null)
        return $"{UserClaimModel.LastName}, {UserClaimModel.FirstName}";
      return null;
    }
  }

  public UserClaimModel UserClaimModel { get; set; } = default!;

  public UserPolicies? UserPolicies { get { return UserClaimModel?.UserPolicies; } }

  public void Setup(UserClaimModel ucm)
  {
    UserClaimModel = ucm;
    IsSetup = true;
  }
}
