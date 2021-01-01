using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RocketLaunchJournal.Model.UserIdentity
{
    public class UserTokenBase
    {
        /// <summary>
        /// Gets or sets the primary key of the user that the token belongs to.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the LoginProvider this token is from.
        /// </summary>
        [StringLength(2000)]
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the name of the token.
        /// </summary>
        [StringLength(1000)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        [StringLength(2000)]
        public string Value { get; set; }
    }

    public class UserToken : UserTokenBase
    {
        #region Navigation Links

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        #endregion
    }
}
