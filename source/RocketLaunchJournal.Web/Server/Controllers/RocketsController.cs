using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Services;
using RocketLaunchJournal.Infrastructure.Services.Rockets;
using RocketLaunchJournal.Web.Shared.UserIdentity.Policies;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RocketsController : BaseController
    {
        private readonly ILogger<RocketsController> logger;

        public RocketsController(ILogger<RocketsController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RocketDto>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var service = GetService<RocketsGet>();
            return Ok(await service.GetRockets(false));
        }

        [Authorize(Policy = PolicyNames.RocketAddEditDelete)]
        [HttpGet("ForSelection")]
        [ProducesResponseType(typeof(List<RocketDto>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> GetForSelection()
        {
            var service = GetService<RocketsGet>();
            return Ok(await service.GetRocketsForSelection());
        }

        /// <summary>
        /// Create a Rocket
        /// </summary>
        /// <param name="dto">user object</param>
        /// <returns>updated user object</returns>
        [HttpPost]
        [Authorize(Policy = PolicyNames.RocketAddEditDelete)]
        [ProducesResponseType(typeof(RocketDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult?> Create([FromBody] RocketDto dto)
        {
            return await SaveRocket(dto);
        }

        /// <summary>
        /// Update a Rocket
        /// </summary>
        /// <param name="dto">user object</param>
        /// <returns>updated user object</returns>
        [HttpPut]
        [Authorize(Policy = PolicyNames.RocketAddEditDelete)]
        [ProducesResponseType(typeof(RocketDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult?> Update([FromBody] RocketDto dto)
        {
            return await SaveRocket(dto);
        }

        /// <summary>
        /// Delete or inactivate
        /// </summary>
        /// <param name="id">id to delete or inactivate</param>
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.RocketAddEditDelete)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult?> Delete(int id)
        {
            if (id <= 0)
                return CreateResponse(new BaseServiceResponse<int>(id, System.Net.HttpStatusCode.BadRequest));

            var service = GetService<RocketsCreateUpdate>();
            return CreateResponse(await service.DeleteRocket(id));
        }

        #region private helpers

        private async Task<IActionResult?> SaveRocket(RocketDto dto)
        {
            if (!ModelState.IsValid)
                return CreateResponse(new BaseServiceResponse<RocketDto>(dto, System.Net.HttpStatusCode.BadRequest));

            using (var transaction = await DBContext.Database.BeginTransactionAsync())
            {
                var service = GetService<RocketsCreateUpdate>();
                var response = await service.SaveRocket(dto);

                transaction.Commit();

                return CreateResponse(response);
            }
        }

        #endregion
    }
}
