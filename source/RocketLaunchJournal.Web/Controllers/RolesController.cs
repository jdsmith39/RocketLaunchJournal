using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Services.Users;

namespace RocketLaunchJournal.Web.Controllers;

/// <summary>
/// Roles controller
/// </summary>
[Authorize]
[Route("api/[controller]")]
public class RolesController : BaseController
{
  /// <summary>
  /// Get roles
  /// </summary>
  /// <returns>List of roles</returns>
  [HttpGet]
  [ProducesResponseType(typeof(List<RoleDto>), (int)System.Net.HttpStatusCode.OK)]
  public async Task<IActionResult> GetGrantableRoles()
  {
    var service = GetService<UsersGet>();
    return Ok(await service.GetGrantableRoles());
  }
}
