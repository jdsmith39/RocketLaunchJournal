using RocketLaunchJournal.Model;
using System;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Services.Logs
{
    public class APILogsCreateUpdate : BaseService
    {
        public APILogsCreateUpdate()
        {
        }

        /// <summary>
        /// Saves an API log
        /// </summary>
        /// <param name="dto">API log to save</param>
        public async System.Threading.Tasks.Task SaveLog(APILog dto)
        {
            var timestamp = DateTime.UtcNow;
            if (dto.APILogId == 0)
            {
                dto.TransmissionDateTime = timestamp;
                LoggingDb.APILogs.Add(dto);
            }
            else
            {
                LoggingDb.APILogs.Attach(dto);
                dto.ResponseDateTime = timestamp;
                LoggingDb.Entry(dto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            await LoggingDb.SaveChangesAsync();
        }
    }
}
