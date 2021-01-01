using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.Services;
using RocketLaunchJournal.Infrastructure.UserIdentity;

namespace RocketLaunchJournal.Web.Server.Controllers
{
    /// <summary>
    /// Controller all other controllers are based on
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = false)]
    public class BaseController : ControllerBase
    {
        private UserPermissionService? _userPermissionService;

        /// <summary>
        /// User permission service.  It is injected
        /// </summary>
        public UserPermissionService UserPermissionService
        {
            get
            {
                if (_userPermissionService == null)
                    _userPermissionService = HttpContext.RequestServices.GetService(typeof(UserPermissionService)) as UserPermissionService;

                return _userPermissionService!;
            }
        }

        /// <summary>
        /// Base Url of the site
        /// </summary>
        public string? BaseUrl
        {
            get
            {
                if (Request == null)
                    return null;

                return $"{Request.Scheme}://{Request.Host.ToString()}/";
            }
        }

        private DataContext? _dbContext;
        /// <summary>
        /// Data context injected in so we can do transactions easier.
        /// </summary>
        public DataContext DBContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;
                }

                return _dbContext!;
            }
        }

        private ILoggingContext? _loggingContext;
        /// <summary>
        /// Data context used for logging only
        /// </summary>
        public ILoggingContext LoggingContext
        {
            get
            {
                if (_loggingContext == null)
                {
                    _loggingContext = HttpContext.RequestServices.GetService(typeof(ILoggingContext)) as ILoggingContext;
                }

                return _loggingContext!;
            }
        }

        /// <summary>
        /// Able to get an injected service and pre setup the Data context and User permission service
        /// </summary>
        /// <typeparam name="T">Type of the service</typeparam>
        /// <returns>returns the service</returns>
        protected T GetService<T>() where T : BaseService
        {
            var service = (T)HttpContext.RequestServices.GetService(typeof(T));
            service.SetupService(DBContext, LoggingContext, UserPermissionService);
            return service;
        }

        /// <summary>
        /// Looks a the response from the service and creates the corresponding response.
        /// </summary>
        /// <typeparam name="T">Data object that would be passed back on OK</typeparam>
        /// <param name="response">BaseServiceResponse</param>
        protected IActionResult CreateResponse<T>(BaseServiceResponse<T> response)
        {
            // there are many status codes
            // add http status codes as needed
            switch (response.Status)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    // if message is empty then check ModelState
                    if (string.IsNullOrEmpty(response.Message) && !ModelState.IsValid)
                    {
                        response.Message = string.Join("<br/>", (from ms in ModelState
                                                                 from e in ms.Value.Errors
                                                                 select $"{e.ErrorMessage}"));
                    }
                    return StatusCode((int)response.Status, response.Message);
                case System.Net.HttpStatusCode.Conflict:
                case System.Net.HttpStatusCode.NotImplemented:
                case System.Net.HttpStatusCode.Unauthorized:
                    return StatusCode((int)response.Status, response.Message);
                case System.Net.HttpStatusCode.OK:
                    return Ok(response.Data);
            }
            return Problem("Unknown reponse type");
        }
    }
}
