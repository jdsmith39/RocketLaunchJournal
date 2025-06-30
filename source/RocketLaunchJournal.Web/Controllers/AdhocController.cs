using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using RocketLaunchJournal.Infrastructure.Services.Adhoc;
using RocketLaunchJournal.Web.Shared.UserIdentity.Policies;
using System.Security.Cryptography;

namespace RocketLaunchJournal.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AdhocController : BaseController
{
  private readonly ILogger<AdhocController> logger;

  public AdhocController(ILogger<AdhocController> logger)
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
  [Authorize(Policy = PolicyNames.ReportAddEditDelete)]
  [ProducesResponseType(typeof(ReportDto), (int)System.Net.HttpStatusCode.OK)]
  public async Task<IActionResult> SaveReport([FromBody] ReportDto dto)
  {
    var service = GetService<AdhocCreateUpdate>();
    return CreateResponse(await service.SaveReport(dto));
  }

  [HttpDelete("Report/{id}")]
  [Authorize(Policy = PolicyNames.ReportAddEditDelete)]
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
  public async Task<IActionResult> GetReportSourceColumns([FromBody] ReportSourceDto dto)
  {
    var service = GetService<AdhocGet>();
    return Ok(await service.GetReportSourceColumns(dto));
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
