using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Model.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace RocketLaunchJournal.Model;

public class LaunchBase
{
  [Key]
  public int LaunchId { get; set; }

  [Display(Name = "Rocket")]
  [Range(1, int.MaxValue, ErrorMessage = "Rocket is required")]
  public int RocketId { get; set; }

  [Display(Name = "Launch #")]
  public int LaunchNumber { get; set; }

  [DataType(DataType.Date)]
  public DateTime Date { get; set; }

  [Required]
  [StringLength(FieldSizes.NameLength)]
  public string? Motors { get; set; }

  [Display(ShortName = "Alt")]
  [Precision(18,6)]
  public decimal? Altitude { get; set; }

  [Display(Name = "Top Speed", ShortName = "TS")]
  [Precision(18, 6)]
  public decimal? TopSpeed { get; set; }

  [Display(Name = "Burn Time", ShortName = "Burn")]
  [Precision(18, 6)]
  public decimal? BurnTime { get; set; }

  [Display(Name = "Peak Acceleration", ShortName = "PAcc")]
  [Precision(18, 6)]
  public decimal? PeakAcceleration { get; set; }

  [Display(Name = "Average Acceleration", ShortName = "AAcc")]
  [Precision(18, 6)]
  public decimal? AverageAcceleration { get; set; }

  [Display(Name = "Coast To Apogee Time", ShortName = "C2AP")]
  [Precision(18, 6)]
  public decimal? CoastToApogeeTime { get; set; }

  [Display(Name = "Apogee To Ejection Time", ShortName = "A2EJ")]
  [Precision(18, 6)]
  public decimal? ApogeeToEjectionTime { get; set; }

  [Display(Name = "Ejection Altitude", ShortName = "EAlt")]
  [Precision(18, 6)]
  public decimal? EjectionAltitude { get; set; }

  [Display(Name = "Descent Speed", ShortName = "Desc")]
  [Precision(18, 6)]
  public decimal? DescentSpeed { get; set; }

  [Display(ShortName = "Dura")]
  [Precision(18, 6)]
  public decimal? Duration { get; set; }

  [Display(Name = "Recovery Notes")]
  [StringLength(FieldSizes.NameLength)]
  public string? RecoveryNotes { get; set; }

  [StringLength(FieldSizes.DescriptionLength)]
  public string? Note { get; set; }
}

[Attributes.AuditLog(Enums.LogTypeEnum.Launch)]
public class Launch : LaunchBase
{
  public OwnedTypes.AuditFields? AuditFields { get; set; }

  #region Navigation Links

  public virtual Rocket? Rocket { get; set; }

  #endregion
}
