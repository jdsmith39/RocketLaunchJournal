using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Dtos.Users;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.OwnedTypes;
using RocketLaunchJournal.Model;

namespace RocketLaunchJournal.Infrastructure.Services.Launches
{
    public class LaunchesCreateUpdate : BaseService
    {
        public LaunchesCreateUpdate()
        {
        }

        /// <summary>
        /// Saves a user
        /// </summary>
        /// <param name="dto">dto to save</param>
        /// <returns>updated dto object</returns>
        public async Task<BaseServiceResponse<T>> SaveLaunch<T>(T dto) where T : LaunchDto
        {
            var response = new BaseServiceResponse<T>(dto);
            // make sure user has access to this
            if (!UserPermissionService.UserPolicies!.LaunchAddEditDelete)
            {
                response.Message = "You are not authorized to add/edit a launch.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            // nothing changed, return
            if (!dto.IsUpdated)
                return response;

            // make sure user has access to the rocket

            var timestamp = DateTime.UtcNow;
            Launch dbObj;
            var isNew = dto.LaunchId == 0;
            if (isNew)
            {
                dbObj = new Launch()
                {
                    AuditFields = new AuditFields(UserPermissionService.UserClaimModel.UserId, timestamp),
                };
                db.Launches.Add(dbObj);
            }
            else
            {
                dbObj = await db.RoleRestrictedLaunches(UserPermissionService, false)
                    .SingleOrDefaultAsync(w => w.LaunchId == dto.LaunchId);
                if (dbObj == null)
                {
                    response.Message = "You are not authorized to edit this launch.";
                    response.Status = System.Net.HttpStatusCode.Unauthorized;
                    return response;
                }
            }

            dbObj.Altitude = dto.Altitude;
            dbObj.ApogeeToEjectionTime = dto.ApogeeToEjectionTime;
            dbObj.AverageAcceleration = dto.AverageAcceleration;
            dbObj.BurnTime = dto.BurnTime;
            dbObj.CoastToApogeeTime = dto.CoastToApogeeTime;
            dbObj.Date = dto.Date;
            dbObj.DescentSpeed = dto.DescentSpeed;
            dbObj.Duration = dto.Duration;
            dbObj.EjectionAltitude = dto.EjectionAltitude;
            dbObj.LaunchNumber = dto.LaunchNumber;
            dbObj.Motors = dto.Motors;
            dbObj.Note = dto.Note;
            dbObj.PeakAcceleration = dto.PeakAcceleration;
            dbObj.RocketId = dto.RocketId;
            dbObj.TopSpeed = dto.TopSpeed;

            dbObj.AuditFields!.SetActiveInactive(dto.IsActive, UserPermissionService.UserClaimModel.UserId, timestamp);

            dbObj.AuditFields.SetUpdated(UserPermissionService.UserClaimModel.UserId, timestamp);

            await db.SaveChangesAsync();

            if (isNew)
            {
                dto.LaunchId = dbObj.LaunchId;
                dto.UserId = UserPermissionService.UserClaimModel.UserId;
            }

            dto.IsUpdated = false;

            return response;
        }

        /// <summary>
        /// Deletes or inactivates a Launch (based on client needs)
        /// </summary>
        /// <param name="id">id to delete</param>
        public async Task<BaseServiceResponse<int>> DeleteLaunch(int id)
        {
            var response = new BaseServiceResponse<int>(id);

            // make sure user has access to this
            if (!UserPermissionService.UserClaimModel.UserPolicies.LaunchAddEditDelete)
            {
                response.Message = "You are not authorized to add/edit a Launch.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            // make sure logged in user has permission to the requested user
            var dbObj = await db.RoleRestrictedLaunches(UserPermissionService, false).FirstOrDefaultAsync(w => w.LaunchId == id);
            if (dbObj == null)
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                return response;
            }
            // delete (need to delete owned types and dependent tables)
            db.Remove(dbObj);
            db.Remove(dbObj.AuditFields);
            // inactivate
            //dbObj.AuditFields.SetActiveInactive(false, UserPermissionService.UserClaimModel.UserId, DateTime.UtcNow);

            await db.SaveChangesAsync();

            return response;
        }
    }
}
