﻿using RocketLaunchJournal.Model.Constants;
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
  public decimal? Altitude { get; set; }

  [Display(Name = "Top Speed", ShortName = "TS")]
  public decimal? TopSpeed { get; set; }

  [Display(Name = "Burn Time", ShortName = "Burn")]
  public decimal? BurnTime { get; set; }

  [Display(Name = "Peak Acceleration", ShortName = "PAcc")]
  public decimal? PeakAcceleration { get; set; }

  [Display(Name = "Average Acceleration", ShortName = "AAcc")]
  public decimal? AverageAcceleration { get; set; }

  [Display(Name = "Coast To Apogee Time", ShortName = "C2AP")]
  public decimal? CoastToApogeeTime { get; set; }

  [Display(Name = "Apogee To Ejection Time", ShortName = "A2EJ")]
  public decimal? ApogeeToEjectionTime { get; set; }

  [Display(Name = "Ejection Altitude", ShortName = "EAlt")]
  public decimal? EjectionAltitude { get; set; }

  [Display(Name = "Descent Speed", ShortName = "Desc")]
  public decimal? DescentSpeed { get; set; }

  [Display(ShortName = "Dura")]
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
