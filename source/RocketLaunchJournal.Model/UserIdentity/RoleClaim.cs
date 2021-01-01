using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;

namespace RocketLaunchJournal.Model.UserIdentity
{
    public class RoleClaimBase
    {
        /// <summary>
        /// Gets or sets the identifier for this role claim.
        /// </summary>
        [Key]
        public int RoleClaimId { get; set; }

        /// <summary>
        /// Gets or sets the of the primary key of the role associated with this claim.
        /// </summary>
        public byte RoleId { get; set; }

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        [Required]
        [StringLength(Constants.FieldSizes.ClaimTypeLength)]
        public string? ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        [Required]
        [StringLength(Constants.FieldSizes.ClaimValueLength)]
        public string? ClaimValue { get; set; }

        /// <summary>
        /// Constructs a new claim with the type and value.
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }

    public class RoleClaim : RoleClaimBase
    {
        #region Navigation Links

        [ForeignKey(nameof(UserIdentity.Role.RoleId))]
        public virtual Role? Role { get; set; }

        #endregion

        /// <summary>
        /// Initializes by copying ClaimType and ClaimValue from the other claim.
        /// </summary>
        /// <param name="other">The claim to initialize from.</param>
        public void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
    }
}
