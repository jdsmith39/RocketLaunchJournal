using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model;
using RocketLaunchJournal.Model.OwnedTypes;
using System;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Services.Rockets;

public class RocketsCreateUpdate : BaseService
{
  /// <summary>
  /// Saves a user
  /// </summary>
  /// <param name="dto">dto to save</param>
  /// <returns>updated dto object</returns>
  public async Task<BaseServiceResponse<T>> SaveRocket<T>(T dto) where T : RocketDto
  {
    var response = new BaseServiceResponse<T>(dto);
    // make sure user has access to this
    if (!UserPermissionService.UserPolicies!.RocketAddEditDelete)
    {
      response.Message = "You are not authorized to add/edit a rocket.";
      response.Status = System.Net.HttpStatusCode.Unauthorized;
      return response;
    }

    // nothing changed, return
    if (!dto.IsUpdated)
      return response;

    var timestamp = DateTime.UtcNow;
    Rocket? dbObj;
    var isNew = dto.RocketId == 0;
    if (isNew)
    {
      dbObj = new Rocket()
      {
        AuditFields = new AuditFields(UserPermissionService.UserClaimModel.UserId, timestamp),
        UserId = UserPermissionService.UserClaimModel.UserId,
      };
      db.Rockets.Add(dbObj);
    }
    else
    {
      dbObj = await db.RoleRestrictedRockets(UserPermissionService, false).SingleOrDefaultAsync(w => w.RocketId == dto.RocketId);
      if (dbObj == null)
      {
        response.Message = "You are not authorized to edit this rocket.";
        response.Status = System.Net.HttpStatusCode.Unauthorized;
        return response;
      }
    }

    dbObj.TubeLengthForApogeeCharge = dto.TubeLengthForApogeeCharge;
    dbObj.BlackPowderForApogee = dto.BlackPowderForApogee;
    dbObj.TubeLengthForMainCharge = dto.TubeLengthForMainCharge;
    dbObj.BlackPowderForMain = dto.BlackPowderForMain;
    dbObj.CenterOfGravity = dto.CenterOfGravity;
    dbObj.CenterOfPreassure = dto.CenterOfPreassure;
    dbObj.Diameter = dto.Diameter;
    dbObj.Length = dto.Length;
    dbObj.Name = dto.Name;
    dbObj.Recovery = dto.Recovery;
    dbObj.Weight = dto.Weight;

    dbObj.AuditFields!.SetActiveInactive(dto.IsActive, UserPermissionService.UserClaimModel.UserId, timestamp);

    dbObj.AuditFields.SetUpdated(UserPermissionService.UserClaimModel.UserId, timestamp);

    await db.SaveChangesAsync();

    if (isNew)
    {
      dto.RocketId = dbObj.RocketId;
      dto.UserId = dbObj.UserId;
    }

    dto.IsUpdated = false;

    return response;
  }

  /// <summary>
  /// Deletes or inactivates a rocket (based on client needs)
  /// </summary>
  /// <param name="id">id to delete</param>
  public async Task<BaseServiceResponse<int>> DeleteRocket(int id)
  {
    var response = new BaseServiceResponse<int>(id);

    // make sure user has access to this
    if (!UserPermissionService.UserClaimModel.UserPolicies.UserAddEditDelete)
    {
      response.Message = "You are not authorized to add/edit a rocket.";
      response.Status = System.Net.HttpStatusCode.Unauthorized;
      return response;
    }

    // make sure logged in user has permission to the requested user
    var dbObj = await db.RoleRestrictedRockets(UserPermissionService, false).FirstOrDefaultAsync(w => w.RocketId == id);
    if (dbObj == null)
    {
      response.Status = System.Net.HttpStatusCode.BadRequest;
      return response;
    }
    // delete (need to delete owned types and dependent tables)
    //db.Remove(dbObj);
    //db.Remove(dbObj.AuditFields);
    // inactivate
    dbObj.AuditFields.SetActiveInactive(false, UserPermissionService.UserClaimModel.UserId, DateTime.UtcNow);

    await db.SaveChangesAsync();

    return response;
  }
}
