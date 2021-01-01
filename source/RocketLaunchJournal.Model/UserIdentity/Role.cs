using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RocketLaunchJournal.Model.UserIdentity
{
    public class RoleBase
    {
        /// <summary>
        /// Gets or sets the primary key for this role.
        /// </summary>
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public byte RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
        [StringLength(Constants.FieldSizes.NameLength)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name for this role.
        /// </summary>
        [StringLength(Constants.FieldSizes.NameLength)]
        public string? NormalizedName { get; set; }

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store
        /// </summary>
        [StringLength(Constants.FieldSizes.ConcurrencyStampLength)]
        public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [StringLength(50)]
        public string? Type { get; set; }

        public int Level { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Data { get; set; }
    }

    public class Role : RoleBase
    {
        public const string RoleTypeGeneral = "General";
        public const string Admin = "Admin";

        #region navigation links

        [InverseProperty(nameof(UserRole.Role))]
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        #endregion
    }
}
