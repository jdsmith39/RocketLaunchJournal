using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model;
using RocketLaunchJournal.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Services.Logs
{
    public class SystemLogsCreate : BaseService
    {
        public SystemLogsCreate()
        {
        }

        /// <summary>
        /// Saves a system log with exception
        /// Defaults userid/companyid to UserPermissionService settings
        /// </summary>
        /// <param name="logType">log type</param>
        /// <param name="exception">event description is built based on the exception</param>
        /// <param name="logGroupKey">related log key (Guid)</param>
        public async System.Threading.Tasks.Task SaveLog(LogTypeEnum logType, Exception exception, Guid? logGroupKey = null)
        {
            var userId = UserPermissionService.UserClaimModel.UserId <= 0 ? (int?)null : UserPermissionService.UserClaimModel.UserId;

            await SaveLog(userId, null, logType, exception, logGroupKey);
        }

        /// <summary>
        /// Saves a system log with exception
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="companyId">company id</param>
        /// <param name="logType">log type</param>
        /// <param name="exception">event description is built based on the exception</param>
        /// <param name="logGroupKey">related log key (Guid)</param>
        public async System.Threading.Tasks.Task SaveLog(int? userId, int? companyId, LogTypeEnum logType, Exception exception, Guid? logGroupKey = null)
        {
            var stackTrace = exception.StackTrace;
            var exceptionMessages = new List<string>()
            {
                exception.Message
            };
            while (exception.InnerException != null)
            {
                exceptionMessages.Add(exception.InnerException.Message);
                exception = exception.InnerException;
            }

            var eventDescription = $"Error:  {string.Join(",\n", exceptionMessages)}.  \nStackTrace:  {stackTrace}";

            await SaveLog(userId, null, eventDescription, logType, logGroupKey);
        }

        /// <summary>
        /// Saves a system log
        /// Defaults userid/companyid to UserPermissionService settings
        /// </summary>
        /// <param name="eventDescription">event description</param>
        /// <param name="logType">log type</param>
        /// <param name="relatedLogsKey">related log key (Guid)</param>
        public async System.Threading.Tasks.Task SaveLog(string eventDescription, LogTypeEnum logType, Guid? relatedLogsKey = null)
        {
            var userId = UserPermissionService.UserClaimModel.UserId <= 0 ? (int?)null : UserPermissionService.UserClaimModel.UserId;

            await SaveLog(userId, null, eventDescription, logType, relatedLogsKey);
        }

        /// <summary>
        /// Saves a system log
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="companyId">company id</param>
        /// <param name="eventDescription">event description</param>
        /// <param name="logType">log type</param>
        /// <param name="logGroupKey">related log key (Guid)</param>
        public async System.Threading.Tasks.Task SaveLog(int? userId, int? companyId, string eventDescription, LogTypeEnum logType, Guid? logGroupKey = null)
        {
            var timestamp = DateTime.UtcNow;
            var dbObj = new SystemLog()
            {
                EventDateTime = timestamp,
                EventDescription = eventDescription,
                IpAddress = UserPermissionService.UserClaimModel.IpAddress,
                LogTypeId = logType,
                UserId = userId,
                LogGroupKey = logGroupKey,
            };

            await SaveLog(dbObj);
        }

        /// <summary>
        /// Saves a system log
        /// </summary>
        /// <param name="dto">system log to save</param>
        public async System.Threading.Tasks.Task SaveLog(SystemLog dto)
        {
            LoggingDb.SystemLogs.Add(dto);
            await LoggingDb.SaveChangesAsync();
        }
    }
}
