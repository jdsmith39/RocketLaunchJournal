using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketLaunchJournal.Infrastructure.Dtos.Users;
using RocketLaunchJournal.Infrastructure.Services;
using RocketLaunchJournal.Infrastructure.Services.Users;
using RocketLaunchJournal.Web.Shared.UserIdentity.Policies;
using System.Net;

namespace RocketLaunchJournal.Web.Controllers;

/// <summary>
/// users controller
/// </summary>
[Route("api/[controller]")]
[Authorize(Policy = PolicyNames.UserAddEditDelete)]
public class UsersController : BaseController
{
  /// <summary>
  /// Get users
  /// </summary>
  /// <returns>list of users</returns>
  [HttpGet]
  [Authorize(Policy = PolicyNames.UserAddEditDelete)]
  [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> GetUsers()
  {
    var service = GetService<UsersGet>();
    return Ok(await service.GetUsers(false));
  }

  /// <summary>
  /// Create a user
  /// </summary>
  /// <param name="dto">user object</param>
  /// <returns>updated user object</returns>
  [HttpPost]
  [Authorize(Policy = PolicyNames.UserAddEditDelete)]
  [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
  [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult?> CreateUser([FromBody] UserDto dto)
  {
    return await SaveUser(dto);
  }

  /// <summary>
  /// Update a user
  /// </summary>
  /// <param name="dto">user object</param>
  /// <returns>updated user object</returns>
  [HttpPut]
  [Authorize(Policy = PolicyNames.UserAddEditDelete)]
  [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
  [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult?> UpdateUser([FromBody] UserDto dto)
  {
    return await SaveUser(dto);
  }

  /// <summary>
  /// Delete or inactivate
  /// </summary>
  /// <param name="id">id to delete or inactivate</param>
  [HttpDelete("{id}")]
  [Authorize(Policy = PolicyNames.UserAddEditDelete)]
  [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
  [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult?> DeleteUser(int id)
  {
    if (id <= 0)
      return CreateResponse(new BaseServiceResponse<int>(id, System.Net.HttpStatusCode.BadRequest));

    var service = GetService<UsersCreateUpdate>();
    return CreateResponse(await service.DeleteUser(id));
  }

  #region private helpers

  private async Task<IActionResult?> SaveUser(UserDto dto)
  {
    if (!ModelState.IsValid)
      return CreateResponse(new BaseServiceResponse<UserDto>(dto, System.Net.HttpStatusCode.BadRequest));

    using (var transaction = await DBContext.Database.BeginTransactionAsync())
    {
      var service = GetService<UsersCreateUpdate>();
      var response = await service.SaveUser(dto);

      transaction.Commit();

      return CreateResponse(response);
    }
  }

  #endregion
}
