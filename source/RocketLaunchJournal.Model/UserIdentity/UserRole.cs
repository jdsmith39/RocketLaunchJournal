using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RocketLaunchJournal.Model.UserIdentity
{
    public class UserRoleBase
    {
        public int UserId { get; set; }
        public byte RoleId { get; set; }
    }

    public class UserRole : UserRoleBase
    {
        #region navigation links

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }

        #endregion
    }
}
