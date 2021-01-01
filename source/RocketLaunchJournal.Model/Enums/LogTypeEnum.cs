using RocketLaunchJournal.Model.Attributes;

namespace RocketLaunchJournal.Model.Enums
{
    [TypeWriterIgnore]
    public enum LogTypeEnum : byte
    {
        Unknown = 0,
        LoginInvalidLogin = 1,
        LoginLockOut = 2,
        LoginFindUserName = 3,
        LoginFindEmail = 4,
        LoginSuccess = 5,
        User = 6,
        Address = 7,

        Rocket = 10,
        Launch = 11,

        VerifyOneTimePassword = 101,

        /// <summary>
        /// Log type reserved for unhandled exceptions caught by the middleware
        /// </summary>
        UncaughtError = 200,
    }
}
