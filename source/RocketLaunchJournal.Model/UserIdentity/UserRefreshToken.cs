using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RocketLaunchJournal.Model.UserIdentity
{
    public class UserRefreshTokenBase
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(100)]
        public string RefreshToken { get; set; }

        public DateTime ExpiresOnDateTime { get; set; }

        public int? ImpersonationUserId { get; set; }
    }

    public class UserRefreshToken : UserRefreshTokenBase
    {
        #region Navigation Links

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(ImpersonationUserId))]
        public virtual User ImpersonationUser { get; set; }

        #endregion
    }
}
