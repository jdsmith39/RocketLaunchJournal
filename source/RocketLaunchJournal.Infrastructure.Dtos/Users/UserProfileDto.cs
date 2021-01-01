using RocketLaunchJournal.Model.UserIdentity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RocketLaunchJournal.Infrastructure.Dtos.Users
{
    public class UserProfileDto : UserRoot
    {
        public bool IsUpdated { get; set; }
    }
}
