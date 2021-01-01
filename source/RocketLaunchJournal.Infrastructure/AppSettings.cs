using Microsoft.Extensions.Configuration;

namespace RocketLaunchJournal.Infrastructure
{
    public class AppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
        }

        public const int PasswordMinimumLength = 12;
    }
}
