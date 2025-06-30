using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Infrastructure.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Services.Launches;

public class LaunchesGet : BaseService
{
  /// <summary>
  /// Gets all launches that the logged in user has access to.
  /// </summary>
  /// <param name="activeOnly">active only items</param>
  public Task<List<LaunchDto>> GetLaunches(bool activeOnly)
  {
    return (from x in db.LaunchesByFilter(UserPermissionService, activeOnly)
            orderby x.Rocket.Name, x.LaunchNumber
            select new LaunchDto()
            {
              Name = x.Rocket.Name,
              UserId = x.Rocket.UserId,
              LaunchId = x.LaunchId,
              Altitude = x.Altitude,
              ApogeeToEjectionTime = x.ApogeeToEjectionTime,
              AverageAcceleration = x.AverageAcceleration,
              BurnTime = x.BurnTime,
              CoastToApogeeTime = x.CoastToApogeeTime,
              Date = x.Date,
              DescentSpeed = x.DescentSpeed,
              Duration = x.Duration,
              EjectionAltitude = x.EjectionAltitude,
              LaunchNumber = x.LaunchNumber,
              Motors = x.Motors,
              Note = x.Note,
              PeakAcceleration = x.PeakAcceleration,
              RocketId = x.RocketId,
              TopSpeed = x.TopSpeed,
              RecoveryNotes = x.RecoveryNotes,
              IsActive = !x.AuditFields.InactiveDateTime.HasValue,
            }).ToListAsync();
  }

  /// <summary>
  /// Gets all launches that the logged in user has access to.
  /// </summary>
  /// <param name="activeOnly">active only items</param>
  public Task<List<LaunchDto>> GetLaunchesByRocketId(bool activeOnly, int id)
  {
    return (from x in db.LaunchesByFilter(UserPermissionService, activeOnly)
            where x.RocketId == id
            orderby x.Rocket.Name, x.LaunchNumber
            select new LaunchDto()
            {
              Name = x.Rocket.Name,
              UserId = x.Rocket.UserId,
              LaunchId = x.LaunchId,
              Altitude = x.Altitude,
              ApogeeToEjectionTime = x.ApogeeToEjectionTime,
              AverageAcceleration = x.AverageAcceleration,
              BurnTime = x.BurnTime,
              CoastToApogeeTime = x.CoastToApogeeTime,
              Date = x.Date,
              DescentSpeed = x.DescentSpeed,
              Duration = x.Duration,
              EjectionAltitude = x.EjectionAltitude,
              LaunchNumber = x.LaunchNumber,
              Motors = x.Motors,
              Note = x.Note,
              PeakAcceleration = x.PeakAcceleration,
              RocketId = x.RocketId,
              TopSpeed = x.TopSpeed,
              RecoveryNotes = x.RecoveryNotes,
              IsActive = !x.AuditFields.InactiveDateTime.HasValue,
            }).ToListAsync();
  }

  /// <summary>
  /// Gets recent launches that the logged in user has access to.
  /// </summary>
  public Task<List<LaunchDto>> GetRecentLaunches()
  {
    return (from x in db.LaunchesByFilter(UserPermissionService, true)
            orderby x.Date descending, x.Rocket.Name
            select new LaunchDto()
            {
              Name = x.Rocket.Name,
              UserId = x.Rocket.UserId,
              LaunchId = x.LaunchId,
              Altitude = x.Altitude,
              ApogeeToEjectionTime = x.ApogeeToEjectionTime,
              AverageAcceleration = x.AverageAcceleration,
              BurnTime = x.BurnTime,
              CoastToApogeeTime = x.CoastToApogeeTime,
              Date = x.Date,
              DescentSpeed = x.DescentSpeed,
              Duration = x.Duration,
              EjectionAltitude = x.EjectionAltitude,
              LaunchNumber = x.LaunchNumber,
              Motors = x.Motors,
              Note = x.Note,
              PeakAcceleration = x.PeakAcceleration,
              RocketId = x.RocketId,
              TopSpeed = x.TopSpeed,
              RecoveryNotes = x.RecoveryNotes,
              IsActive = !x.AuditFields.InactiveDateTime.HasValue,
            }).Take(20).ToListAsync();
  }
}
