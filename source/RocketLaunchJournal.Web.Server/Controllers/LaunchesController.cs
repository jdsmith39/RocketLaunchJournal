using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Services;
using RocketLaunchJournal.Infrastructure.Services.Launches;
using RocketLaunchJournal.Web.Shared.UserIdentity.Policies;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LaunchesController : BaseController
    {
        private readonly ILogger<LaunchesController> logger;

        public LaunchesController(ILogger<LaunchesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<LaunchDto>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var service = GetService<LaunchesGet>();
            return Ok(await service.GetLaunches(false));
        }

        [HttpGet("{rocketId}")]
        [ProducesResponseType(typeof(List<LaunchDto>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> GetByRocketId(int rocketId)
        {
            var service = GetService<LaunchesGet>();
            return Ok(await service.GetLaunchesByRocketId(false, rocketId));
        }

        [HttpGet("recent")]
        [ProducesResponseType(typeof(List<LaunchDto>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> GetRecentLaunches()
        {
            var service = GetService<LaunchesGet>();
            return Ok(await service.GetRecentLaunches());
        }

        /// <summary>
        /// Create a Rocket
        /// </summary>
        /// <param name="dto">user object</param>
        /// <returns>updated user object</returns>
        [HttpPost]
        [Authorize(Policy = PolicyNames.LaunchAddEditDelete)]
        [ProducesResponseType(typeof(LaunchDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult?> Create([FromBody] LaunchDto dto)
        {
            return await SaveRocket(dto);
        }

        /// <summary>
        /// Update a Rocket
        /// </summary>
        /// <param name="dto">user object</param>
        /// <returns>updated user object</returns>
        [HttpPut]
        [Authorize(Policy = PolicyNames.LaunchAddEditDelete)]
        [ProducesResponseType(typeof(LaunchDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult?> Update([FromBody] LaunchDto dto)
        {
            return await SaveRocket(dto);
        }

        /// <summary>
        /// Delete or inactivate
        /// </summary>
        /// <param name="id">id to delete or inactivate</param>
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.LaunchAddEditDelete)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult?> Delete(int id)
        {
            if (id <= 0)
                return CreateResponse(new BaseServiceResponse<int>(id, System.Net.HttpStatusCode.BadRequest));

            var service = GetService<LaunchesCreateUpdate>();
            return CreateResponse(await service.DeleteLaunch(id));
        }

        #region private helpers

        private async Task<IActionResult?> SaveRocket(LaunchDto dto)
        {
            if (!ModelState.IsValid)
                return CreateResponse(new BaseServiceResponse<LaunchDto>(dto, System.Net.HttpStatusCode.BadRequest));

            using (var transaction = await DBContext.Database.BeginTransactionAsync())
            {
                var service = GetService<LaunchesCreateUpdate>();
                var response = await service.SaveLaunch(dto);

                transaction.Commit();

                return CreateResponse(response);
            }
        }

        #endregion
    }
}
