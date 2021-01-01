using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RocketLaunchJournal.Model.UserIdentity;

namespace RocketLaunchJournal.Infrastructure.Dtos.Users
{
    public class UserDto : UserRoot
    {
        [Display(Name = "Role")]
        public RoleDto? Role { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public bool IsUpdated { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is UserDto user)
            {
                if (Role == null && user.Role != null)
                    return false;

                var roleEqual = (Role == null && user.Role == null) || (Role != null && Role.Equals(user.Role));

                return UserId == user.UserId && FirstName == user.FirstName && LastName == user.LastName
                    && Email == user.Email && IsActive == user.IsActive && IsLoginEnabled == user.IsLoginEnabled
                    && PhoneNumber == user.PhoneNumber && roleEqual;
            }

            return false;
        }
    }
}
