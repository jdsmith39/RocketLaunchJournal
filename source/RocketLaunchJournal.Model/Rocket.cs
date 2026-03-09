using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Model.Constants;
using RocketLaunchJournal.Model.UserIdentity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RocketLaunchJournal.Model;

public class RocketBase
{
  [Key]
  public int RocketId { get; set; }

  [Required(ErrorMessage = ErrorMessages.RequiredField), StringLength(FieldSizes.NameLength, ErrorMessage = ErrorMessages.StringLengthMax)]
  public string? Name { get; set; }

  [Display(Name = "Weight", ShortName = "WT")]
  [Precision(18, 6)]
  public decimal? Weight { get; set; }

  [Display(Name = "Diameter", ShortName = "Dia")]
  [Precision(18, 6)]
  public decimal? Diameter { get; set; }

  [Display(Name = "Length", ShortName = "Len")]
  [Precision(18, 6)]
  public decimal? Length { get; set; }

  [Display(Name = "Recovery")]
  [StringLength(FieldSizes.NameLength)]
  public string? Recovery { get; set; }

  [Display(Name= "Tube Length For Apogee Charge")]
  [Precision(18, 6)]
  public decimal? TubeLengthForApogeeCharge { get; set; }

  [Display(Name = "Black Powder for Apogee", ShortName = "BPAP")]
  [Precision(18, 6)]
  public decimal? BlackPowderForApogee { get; set; }

  [Display(Name = "Tube Length For Main Charge")]
  [Precision(18, 6)]
  public decimal? TubeLengthForMainCharge { get; set; }

  [Display(Name = "Black Powder for Main", ShortName = "BPM")]
  [Precision(18, 6)]
  public decimal? BlackPowderForMain { get; set; }

  [Display(Name ="Center of Gravity")]
  [Precision(18, 6)]
  public decimal? CenterOfGravity { get; set; }

  [Display(Name = "Center of Preassure")]
  [Precision(18, 6)]
  public decimal? CenterOfPreassure { get; set; }

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
