namespace RocketLaunchJournal.Web.Shared.UserIdentity.Policies
{
    /// <summary>
    /// Names of all policies within the system
    /// </summary>
    public static class PolicyNames
    {
        public const string CanImpersonate = nameof(Policies.CanImpersonate);
        public const string LaunchAddEditDelete = nameof(Policies.LaunchAddEditDelete);
        public const string ReportAddEditDelete = nameof(Policies.ReportAddEditDelete);
        public const string RocketAddEditDelete = nameof(Policies.RocketAddEditDelete);
        public const string UserAddEditDelete = nameof(Policies.UserAddEditDelete);
        public const string UserProfileEdit = nameof(Policies.UserProfileEdit);
    }
}
