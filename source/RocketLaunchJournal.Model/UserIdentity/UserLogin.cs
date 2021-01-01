using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RocketLaunchJournal.Model.UserIdentity
{
    public class UserLoginBase
    {
        /// <summary>
        /// Gets or sets the login provider for the login (e.g. facebook, google)
        /// </summary>
        [StringLength(2000)]
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the unique provider identifier for this login.
        /// </summary>
        [StringLength(2000)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the friendly name used in a UI for this login.
        /// </summary>
        [StringLength(2000)]
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the of the primary key of the user associated with this login.
        /// </summary>
        public int UserId { get; set; }
    }

    public class UserLogin : UserLoginBase
    {
        #region Navigation Links

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        #endregion
    }
}
