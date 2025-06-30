using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model;
using RocketLaunchJournal.Model.Adhoc;
using RocketLaunchJournal.Model.OwnedTypes;
using System;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Services.Adhoc
{
    public class AdhocCreateUpdate : BaseService
    {
        public AdhocCreateUpdate()
        {
        }

        /// <summary>
        /// Saves a user
        /// </summary>
        /// <param name="dto">dto to save</param>
        /// <returns>updated dto object</returns>
        public async Task<BaseServiceResponse<T>> SaveReport<T>(T dto) where T : ReportDto
        {
            var response = new BaseServiceResponse<T>(dto);
            // make sure user has access to this
            if (!UserPermissionService.UserPolicies!.ReportAddEditDelete)
            {
                response.Message = "You are not authorized to add/edit a report.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            // nothing changed, return
            if (!dto.IsUpdated)
                return response;

            var timestamp = DateTime.UtcNow;
            Report? dbObj;
            var isNew = dto.ReportId == 0;
            if (isNew)
            {
                if (await db.Reports.AnyAsync(w => dto.Name == w.Name && UserPermissionService.UserClaimModel.UserId == w.UserId))
                {
                    response.Message = "Can't have multiple reports with the same name per user.";
                    response.Status = System.Net.HttpStatusCode.BadRequest;
                    return response;
                }

                dbObj = new Report()
                {
                    AuditFields = new AuditFields(UserPermissionService.UserClaimModel.UserId, timestamp),
                    UserId = UserPermissionService.UserClaimModel.UserId,
                };
                db.Reports.Add(dbObj);
            }
            else
            {
                if (await db.Reports.AnyAsync(w => dto.Name == w.Name && dto.UserId == w.UserId && w.ReportId != dto.ReportId))
                {
                    response.Message = "Can't have multiple reports with the same name per user.";
                    response.Status = System.Net.HttpStatusCode.BadRequest;
                    return response;
                }

                dbObj = await db.RoleRestrictedReports(UserPermissionService, false).SingleOrDefaultAsync(w => w.ReportId == dto.ReportId);
                if (dbObj == null)
                {
                    response.Message = "You are not authorized to edit this report.";
                    response.Status = System.Net.HttpStatusCode.Unauthorized;
                    return response;
                }
            }

            dbObj.IsShared = dto.IsShared;
            dbObj.Name = dto.Name;
            dbObj.ReportSourceId = dto.ReportSourceId;
            dbObj.Data = dto.Columns.SerializeJson();

            dbObj.AuditFields!.SetUpdated(UserPermissionService.UserClaimModel.UserId, timestamp);

            await db.SaveChangesAsync();

            if (isNew)
            {
                dto.ReportId = dbObj.ReportId;
                dto.UserId = dbObj.UserId;
                dto.UserName = $"{UserPermissionService.UserClaimModel.FirstName} {UserPermissionService.UserClaimModel.LastName!.Substring(0, 1)}.";
            }

            dto.IsUpdated = false;

            return response;
        }

        /// <summary>
        /// Deletes or inactivates a report (based on client needs)
        /// </summary>
        /// <param name="id">id to delete</param>
        public async Task<BaseServiceResponse<int>> DeleteReport(int id)
        {
            var response = new BaseServiceResponse<int>(id);

            // make sure user has access to this
            if (!UserPermissionService.UserClaimModel.UserPolicies!.ReportAddEditDelete)
            {
                response.Message = "You are not authorized to add/edit a Report.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            // make sure logged in user has permission to the requested user
            var dbObj = await db.RoleRestrictedReports(UserPermissionService, false).FirstOrDefaultAsync(w => w.ReportId == id);
            if (dbObj == null)
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                return response;
            }
            // delete (need to delete owned types and dependent tables)
            db.Remove(dbObj.AuditFields);
            db.Remove(dbObj);
            // inactivate
            //dbObj.AuditFields.SetActiveInactive(false, UserPermissionService.UserClaimModel.UserId, DateTime.UtcNow);

            await db.SaveChangesAsync();

            return response;
        }
    }
}
