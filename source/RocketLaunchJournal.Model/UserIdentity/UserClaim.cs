using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;

namespace RocketLaunchJournal.Model.UserIdentity
{
    public class UserClaimBase
    {
        /// <summary>
        /// Gets or sets the identifier for this user claim.
        /// </summary>
        [Key]
        public int UserClaimId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the user associated with this claim.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        [Required]
        [StringLength(Constants.FieldSizes.ClaimTypeLength)]
        public string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        [Required]
        [StringLength(Constants.FieldSizes.ClaimValueLength)]
        public string ClaimValue { get; set; }

        /// <summary>
        /// Converts the entity into a Claim instance.
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }

    public class UserClaim : UserClaimBase
    {
        /// <summary>
        /// Reads the type and value from the Claim.
        /// </summary>
        /// <param name="claim"></param>
        public void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        #region Navigation Links

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        #endregion
    }
}
