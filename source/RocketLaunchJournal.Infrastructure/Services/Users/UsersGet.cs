using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Dtos.Users;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLaunchJournal.Model.Enums;
using Microsoft.EntityFrameworkCore.Internal;

namespace RocketLaunchJournal.Infrastructure.Services.Users
{
    public class UsersGet:BaseService
    {
        /// <summary>
        /// Gets all users that the logged in user has access to.
        /// </summary>
        /// <param name="activeOnly">active only items</param>
        public async Task<IEnumerable<UserDto>> GetUsers(bool activeOnly)
        {
            return await (from u in db.UsersByFilter(UserPermissionService, activeOnly)
                          orderby u.LastName, u.FirstName
                          select new UserDto()
                          {
                              UserId = u.UserId,
                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              Email = u.Email,
                              PhoneNumber = u.PhoneNumber,
                              IsActive = !u.AuditFields!.InactiveDateTime.HasValue,
                              IsLoginEnabled = u.IsLoginEnabled,
                              Role = u.UserRoles.Select(r => new RoleDto()
                              {
                                  RoleId = r.RoleId,
                                  Level = r.Role.Level,
                                  Name = r.Role.Name,
                                  Type = r.Role.Type,
                              }).FirstOrDefault(),
                          }).ToListAsync();
        }
        
        /// <summary>
        /// Gets all General roles the logged in user has access to
        /// </summary>
        public async Task<List<RoleDto>> GetGrantableRoles()
        {
            return await (from r in db.RoleRestrictedRoles(UserPermissionService, true)
                          orderby r.Level
                          select new RoleDto()
                          {
                              RoleId = r.RoleId,
                              Level = r.Level,
                              Name = r.Name,
                              Type = r.Type
                          }).ToListAsync();
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">userId to get</param>
        /// <returns>userDto</returns>
        public async Task<UserDto> GetUser(int userId)
        {
            return await (from u in db.RoleRestrictedUsers(UserPermissionService)
                          where u.UserId == userId
                          select new UserDto()
                          {
                              UserId = u.UserId,
                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              Email = u.Email,
                              PhoneNumber = u.PhoneNumber,
                              IsActive = !u.AuditFields!.InactiveDateTime.HasValue,
                              IsLoginEnabled = u.IsLoginEnabled,
                              Role = u.UserRoles.Select(r => new RoleDto()
                              {
                                  RoleId = r.RoleId,
                                  Level = r.Role.Level,
                                  Name = r.Role.Name,
                                  Type = r.Role.Type,
                              }).FirstOrDefault(),
                          }).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets a user profile
        /// </summary>
        /// <param name="userId">userId to get</param>
        /// <returns>userDto</returns>
        public async Task<UserProfileDto> GetUserProfile()
        {
            return await (from u in db.RoleRestrictedUsers(UserPermissionService)
                          where u.UserId == UserPermissionService.UserClaimModel.UserId
                          select new UserProfileDto()
                          {
                              UserId = u.UserId,
                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              Email = u.Email,
                              PhoneNumber = u.PhoneNumber,
                              TwoFactorEnabled = u.TwoFactorEnabled,
                          }).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets all user roles
        /// </summary>
        /// <param name="userId">user id to get roles for</param>
        /// <param name="ups">UserPermissionService</param>
        /// <param name="withRoleData">returns role data with the dto if true</param>
        public async Task<List<RoleDto>> GetUserRoles(int userId, UserPermissionService ups, bool withRoleData = false)
        {
            return await (from u in db.Users
                          join ur in db.UserRoles on u.UserId equals ur.UserId
                          join r in db.RoleRestrictedRoles(UserPermissionService, false) on ur.RoleId equals r.RoleId
                          where !u.AuditFields!.InactiveDateTime.HasValue && u.UserId == userId
                          orderby r.Level
                          select new RoleDto()
                          {
                              RoleId = r.RoleId,
                              Level = r.Level,
                              Name = r.Name,
                              Type = r.Type,
                              Data = withRoleData ? r.Data : null
                          }).ToListAsync();
        }
    }
}
