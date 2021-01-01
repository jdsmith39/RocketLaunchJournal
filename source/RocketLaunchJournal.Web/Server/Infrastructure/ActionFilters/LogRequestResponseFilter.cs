using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.Services.Logs;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Server.Infrastructure.ActionFilters
{
    /// <summary>
    /// Use on any action you want to log the request information and response information
    /// </summary>
    public class LogRequestResponseFilter : ActionFilterAttribute
    {
        private readonly APILogsCreateUpdate _apiLogsCreateUpdate;
        private Model.APILog? apiLog;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="apiLogsCreateUpdate"></param>
        /// <param name="db"></param>
        /// <param name="loggingDb"></param>
        /// <param name="userPermissionService"></param>
        public LogRequestResponseFilter(APILogsCreateUpdate apiLogsCreateUpdate, DataContext db, ILoggingContext loggingDb, UserPermissionService userPermissionService)
        {
            apiLogsCreateUpdate.SetupService(db, loggingDb, userPermissionService);
            _apiLogsCreateUpdate = apiLogsCreateUpdate;
        }

        /// <summary>
        /// logs the request body
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            var body = string.Join("\n\n", context.ActionArguments.Select(s =>
            {
                return CleanObjectAndSerialize(s.Value);
            }));

            if (request.Body.CanSeek)
            {
                var sr = new StreamReader(request.Body);
                body += $"\n\n {await sr.ReadToEndAsync()}";
                request.Body.Position = 0;
            }
            else if (request.Form != null && request.Form.Count > 0)
            {
                body += $"\n\n {string.Join("\n\n", request.Form.Select(s => s))}";
            }

            apiLog = new Model.APILog()
            {
                IncomingRequest = true,
                RequestContentBlock = body,
                TargetURL = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
            };

            await _apiLogsCreateUpdate.SaveLog(apiLog);
            await base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// Logs the response body/status code
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await base.OnResultExecutionAsync(context, next);

            if (apiLog == null)
            {
                var request = context.HttpContext.Request;

                apiLog = new Model.APILog()
                {
                    IncomingRequest = false,
                    RequestContentBlock = "",
                    TargetURL = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                    TransmissionDateTime = DateTime.UtcNow
                };
            }

            switch (context.Result)
            {
                case ObjectResult objectResult:
                    apiLog.ResponseContentBlock = $"StatusCode:{objectResult.StatusCode}\n\n{CleanObjectAndSerialize(objectResult.Value)}";
                    break;
                case StatusCodeResult codeResult:
                    apiLog.ResponseContentBlock = CleanObjectAndSerialize(codeResult.StatusCode);
                    break;
                case ContentResult contentResult:
                    apiLog.ResponseContentBlock = $"StatusCode:{contentResult.StatusCode}\n\n{CleanObjectAndSerialize(contentResult.Content)}";
                    break;
                default:
                    apiLog.ResponseContentBlock = $"Missing result type for:  {context.Result.GetType().ToString()}";
                    break;
            }

            await _apiLogsCreateUpdate.SaveLog(apiLog);
        }

        private string[] propertiesToScrub = new string[] { "Password", "PassCode" };
        private string CleanObjectAndSerialize<T>(T value)
        {
            var typeOfValue = value!.GetType();
            var copyOfValue = value.SerializeJson().DeserializeJson(typeOfValue);
            var properties = value.GetType().GetProperties().Where(w => propertiesToScrub.Contains(w.Name)).ToList();

            foreach (var item in properties)
            {
                if (item.PropertyType == typeof(string))
                    item.SetValue(copyOfValue, null);
            }

            return copyOfValue.SerializeJson();
        }
    }
}
