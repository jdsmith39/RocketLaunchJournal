using RocketLaunchJournal.Infrastructure.UserIdentity;

namespace RocketLaunchJournal.DataSeed
{
    public class UserClaimBuilderTester : UserClaimModel
    {
        public UserClaimBuilderTester()
        {
            UserId = 0;
            IsAdmin = true;

            SetupPolicies();
        }
    }
}
