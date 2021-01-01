﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using RocketLaunchJournal.Infrastructure.Services.Adhoc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AdhocController : BaseController
    {
        private readonly ILogger<RocketsController> logger;

        public AdhocController(ILogger<RocketsController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("Reports")]
        [ProducesResponseType(typeof(List<ReportDto>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> GetReports()
        {
            var service = GetService<AdhocGet>();
            return Ok(await service.GetReports());
        }

        [HttpPost("Report")]
        [HttpPut("Report")]
        [ProducesResponseType(typeof(ReportDto), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> SaveReport([FromBody] ReportDto dto)
        {
            var service = GetService<AdhocCreateUpdate>();
            return CreateResponse(await service.SaveReport(dto));
        }

        [HttpDelete("Report/{id}")]
        [ProducesResponseType(typeof(List<ReportSourceDto>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var service = GetService<AdhocCreateUpdate>();
            return CreateResponse(await service.DeleteReport(id));
        }

        [HttpGet("ReportSources")]
        [ProducesResponseType(typeof(List<ReportSourceDto>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> GetReportSources()
        {
            var service = GetService<AdhocGet>();
            return Ok(await service.GetReportSources());
        }

        [HttpPost("ReportSources/Columns")]
        [ProducesResponseType(typeof(List<ReportSourceColumnDto>), (int)System.Net.HttpStatusCode.OK)]
        public IActionResult GetReportSourceColumns([FromBody] ReportSourceDto dto)
        {
            var service = GetService<AdhocGet>();
            return Ok(service.GetReportSourceColumns(dto));
        }

        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateReport([FromBody] ReportDto dto)
        {
            var service = GetService<AdhocGet>();
            return Ok(await service.GenerateReport(dto));
        }

        [HttpPost("Download")]
        public async Task<IActionResult> DownloadReport([FromBody] ReportDto dto)
        {
            var service = GetService<AdhocGet>();
            var fileDownload = await service.DownloadReport(dto);
            if (fileDownload == null)
                return BadRequest();
            return new FileDownloadAndDeleteFileResult(fileDownload);
        }
    }
}
