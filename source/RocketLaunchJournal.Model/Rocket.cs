using RocketLaunchJournal.Model.Constants;
using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RocketLaunchJournal.Model
{
    public class RocketBase
    {
        [Key]
        public int RocketId { get; set; }

        [Required(ErrorMessage = ErrorMessages.RequiredField), StringLength(FieldSizes.NameLength, ErrorMessage = ErrorMessages.StringLengthMax)]
        public String? Name { get; set; }

        [Display(Name="Weight (g)", ShortName = "WT")]       
        public decimal? Weight { get; set; }

        [Display(Name = "Diameter (mm)",  ShortName = "Dia")]
        public decimal? Diameter { get; set; }

        [Display(Name = "Length (m)", ShortName = "Len")]
        public decimal? Length { get; set; }

        [Display(Name="Recovery")]
        [StringLength(FieldSizes.NameLength)]
        public String? Recovery { get; set; }

        [Display(Name= "Black Powder for Apogee (g)", ShortName = "BPAP")]
        public decimal? BlackPowderForApogee { get; set; }

        [Display(Name = "Black Powder for Main (g)", ShortName = "BPM")]
        public decimal? BlackPowderForMain { get; set; }

        public int UserId { get; set; }
    }

    [Attributes.AuditLog(Enums.LogTypeEnum.Rocket)]
    public class Rocket : RocketBase
    {
        public OwnedTypes.AuditFields? AuditFields { get; set; }

        #region Navigation Links

        public virtual ICollection<Launch>? Launches { get; set; }

        public virtual User? User { get; set; }

        #endregion
    }
}
