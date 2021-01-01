using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using RocketLaunchJournal.Infrastructure;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Server.Infrastructure.ActionFilters
{
    /// <summary>
    /// Action Filter that does Ip white list restricting
    /// Requires "IPAddressRange" Nuget package
    /// </summary>
    public class IpWhiteListFilter : ActionFilterAttribute
    {
        private readonly IpWhiteListSettings _ipWhiteListSettings;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ipWhiteListSettings"></param>
        public IpWhiteListFilter(IOptions<IpWhiteListSettings> ipWhiteListSettings)
        {
            _ipWhiteListSettings = ipWhiteListSettings.Value;
        }

        /// <summary>
        /// ip white list restricting
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // uncomment below line and add "IPAddressRange" Nuget package
            var found = false;
            foreach (var item in _ipWhiteListSettings.IpList.SelectMany(s=>s.Values))
            {
                //if (IPAddressRange.Parse(item).Contains(context.HttpContext.Connection.RemoteIpAddress))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Content = "Access Denied"
                };
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
