using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Dtos.Users;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.OwnedTypes;
using RocketLaunchJournal.Model.UserIdentity;

namespace RocketLaunchJournal.Infrastructure.Services.Users
{
    public class UsersCreateUpdate : BaseService
    {
        public UsersCreateUpdate()
        {
        }

        /// <summary>
        /// Saves a user
        /// </summary>
        /// <param name="dto">dto to save</param>
        /// <returns>updated dto object</returns>
        public async Task<BaseServiceResponse<T>> SaveUser<T>(T dto) where T : UserDto
        {
            var response = new BaseServiceResponse<T>(dto);
            // make sure user has access to this
            if (!UserPermissionService.UserPolicies.UserAddEditDelete)
            {
                response.Message = "You are not authorized to add/edit a user.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            // nothing changed, return
            if (!dto.IsUpdated)
                return response;

            var isNew = dto.UserId == 0;
            // check for duplicate email
            var emailQuery = db.Users.Where(w => w.Email == dto.Email);
            if (!isNew)
                emailQuery = emailQuery.Where(w => w.UserId != dto.UserId);
            if (await emailQuery.AnyAsync())
            {
                response.Message = "Email already in use.";
                response.Status = System.Net.HttpStatusCode.Conflict;
                return response;
            }

            var timestamp = DateTime.UtcNow;
            User dbObj;
            if (isNew)
            {
                dbObj = new User()
                {
                    AuditFields = new AuditFields(UserPermissionService.UserClaimModel.UserId, timestamp),
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    // can only view a company that you are in or are impersonating at the time
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                db.Users.Add(dbObj);
            }
            else
            {
                dbObj = await db.RoleRestrictedUsers(UserPermissionService, false)
                    .SingleOrDefaultAsync(w => w.UserId == dto.UserId);
                if (dbObj == null)
                {
                    response.Message = "You are not authorized to edit this user.";
                    response.Status = System.Net.HttpStatusCode.Unauthorized;
                    return response;
                }
            }

            dbObj.Email = dto.Email;
            dbObj.FirstName = dto.FirstName;
            dbObj.LastName = dto.LastName;
            dbObj.NormalizedEmail = dto.Email!.ToUpper();
            dbObj.PhoneNumber = dto.PhoneNumber?.RemoveNonNumerics();
            dbObj.PhoneNumberConfirmed = !string.IsNullOrWhiteSpace(dbObj.PhoneNumber);
            dbObj.IsLoginEnabled = dto.IsLoginEnabled;

            dbObj.AuditFields!.SetActiveInactive(dto.IsActive, UserPermissionService.UserClaimModel.UserId, timestamp);

            dbObj.AuditFields.SetUpdated(UserPermissionService.UserClaimModel.UserId, timestamp);

            if (dbObj.UserRoles == null)
                dbObj.UserRoles = new List<UserRole>();

            var roles = await db.RoleRestrictedRoles(UserPermissionService, true).ToListAsync();

            if (!isNew)
                await db.ClearUserRoles(dbObj.UserId);

            // user doesn't have permission to grant this role skip it.
            var oldRole = dbObj.UserRoles.FirstOrDefault();
            if (dto.Role != null && roles.Any(w => w.RoleId == dto.Role.RoleId))
            {
                var role = dbObj.UserRoles.FirstOrDefault(w => w.RoleId == dto.Role.RoleId);
                if (role == null)
                {
                    dbObj.UserRoles.Add(new Model.UserIdentity.UserRole() { RoleId = dto.Role.RoleId });
                }
            }

            await db.SaveChangesAsync();

            if (isNew)
                dto.UserId = dbObj.UserId;

            dto.IsUpdated = false;

            return response;
        }

        /// <summary>
        /// Saves a user
        /// </summary>
        /// <param name="dto">dto to save</param>
        /// <returns>updated dto object</returns>
        public async Task<BaseServiceResponse<T>> SaveUserProfile<T>(T dto) where T : UserProfileDto
        {
            var response = new BaseServiceResponse<T>(dto);

            if (!UserPermissionService.UserClaimModel.UserPolicies!.UserProfileEdit)
            {
                response.Message = "You are not authorized to edit the user profile.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            // nothing changed, return
            if (!dto.IsUpdated)
                return response;

            var timestamp = DateTime.UtcNow;

            if (db.Users.Any(w => w.Email == dto.Email && w.UserId != UserPermissionService.UserClaimModel.UserId))
            {
                response.Message = "Email already in use.";
                response.Status = System.Net.HttpStatusCode.Conflict;
                return response;
            }

            var dbObj = await db.RoleRestrictedUsers(UserPermissionService, false).Include(i => i.UserRoles).SingleOrDefaultAsync(w => w.UserId == dto.UserId);
            if (dbObj == null)
            {
                response.Message = "You are not authorized to edit this user.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            dbObj.Email = dto.Email;
            dbObj.FirstName = dto.FirstName;
            dbObj.LastName = dto.LastName;
            dbObj.NormalizedEmail = dto.Email!.ToUpper();
            dbObj.PhoneNumber = dto.PhoneNumber?.RemoveNonNumerics();
            dbObj.PhoneNumberConfirmed = !string.IsNullOrWhiteSpace(dbObj.PhoneNumber);
            dbObj.TwoFactorEnabled = dto.TwoFactorEnabled;

            dbObj.AuditFields!.SetUpdated(UserPermissionService.UserClaimModel.UserId, timestamp);

            await db.SaveChangesAsync();

            dto.IsUpdated = false;

            return response;
        }

        /// <summary>
        /// Deletes or inactivates a user (based on client needs)
        /// </summary>
        /// <param name="id">id to delete</param>
        public async Task<BaseServiceResponse<int>> DeleteUser(int id)
        {
            var response = new BaseServiceResponse<int>(id);

            // make sure user has access to this
            if (!UserPermissionService.UserClaimModel.UserPolicies!.UserAddEditDelete)
            {
                response.Message = "You are not authorized to add/edit a user.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            // make sure logged in user has permission to the requested user
            var dbObj = await db.RoleRestrictedUsers(UserPermissionService, false).FirstOrDefaultAsync(w => w.UserId == id);
            if (dbObj == null)
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                return response;
            }
            // delete (need to delete owned types and dependent tables)
            //db.Remove(dbObj);
            //db.Remove(dbObj.AuditFields);
            // inactivate
            dbObj.AuditFields!.SetActiveInactive(false, UserPermissionService.UserClaimModel.UserId, DateTime.UtcNow);

            await db.SaveChangesAsync();

            return response;
        }

        /// <summary>
        /// Saves a user
        /// </summary>
        /// <param name="dto">dto to save</param>
        /// <returns>updated dto object</returns>
        public async Task<BaseServiceResponse<RegisterDto>> RegisterUser(RegisterDto dto, UserManager<User> userManager, IDbContextTransaction transaction)
        {
            var response = new BaseServiceResponse<RegisterDto>(dto);

            var timestamp = DateTime.UtcNow;
            User dbObj;
            var isNew = dto.UserId == 0;
            if (isNew)
            {
                // validation
                if (db.Users.Any(w => w.Email == dto.Email))
                {
                    response.Message = "Email already in use.";
                    response.Status = System.Net.HttpStatusCode.Conflict;
                    return response;
                }

                dbObj = new User()
                {
                    AuditFields = new AuditFields(UserPermissionService.UserClaimModel.UserId, timestamp),
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsLoginEnabled = true,
                };
                db.Users.Add(dbObj);
            }
            else
            {
                response.Message = "You are not authorized to register a new user.";
                response.Status = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }

            dbObj.Email = dto.Email!.Trim();
            dbObj.FirstName = dto.FirstName!.Trim();
            dbObj.LastName = dto.LastName!.Trim();
            dbObj.NormalizedEmail = dto.Email.Trim().ToUpper();
            dbObj.PhoneNumber = dto.PhoneNumber?.RemoveNonNumerics();
            dbObj.PhoneNumberConfirmed = !string.IsNullOrWhiteSpace(dbObj.PhoneNumber);

            dbObj.AuditFields.SetActiveInactive(true, UserPermissionService.UserClaimModel.UserId, timestamp);

            dbObj.AuditFields.SetUpdated(UserPermissionService.UserClaimModel.UserId, timestamp);

            var passwordResult = await userManager.AddPasswordAsync(dbObj, dto.Password);
            if (!passwordResult.Succeeded)
            {
                response.Message = $"Password not accepted. {String.Join(", ", passwordResult.Errors.Select(s => s.Description))}";
                response.Status = System.Net.HttpStatusCode.BadRequest;
                return response;
            }

            await db.SaveChangesAsync();

            dto.Code = await userManager.GenerateEmailConfirmationTokenAsync(dbObj);

            if (isNew)
                dto.UserId = dbObj.UserId;

            return response;
        }

        /// <summary>
        /// Saves the ip address of the user.
        /// </summary>
        /// <param name="userId">user to save to</param>
        /// <returns>BaseServiceResponse</returns>
        public async Task<BaseServiceResponse<string>> SaveIpAddress(int userId)
        {
            var response = new BaseServiceResponse<string>("");
            if (UserPermissionService.UserClaimModel?.IpAddress == null)
                return response;

            var result = await db.SaveIpAddress(userId, UserPermissionService.UserClaimModel.IpAddress);
            if (result == 0)
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Ip Address could not be updated";
            }

            return response;
        }

        /// <summary>
        /// saves the user's last sign in dateTime
        /// </summary>
        /// <param name="userId">user's id to save</param>
        public async Task<BaseServiceResponse<string>> SaveLastSignInDateTime(int userId)
        {
            var response = new BaseServiceResponse<string>("");

            var result = await db.SaveLastSignInDateTime(userId);
            if (result == 0)
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Last sign in date could not be updated";
            }

            return response;
        }

        /// <summary>
        /// saves the user's password change date/time
        /// </summary>
        /// <param name="userId">user's id to save</param>
        public async Task<BaseServiceResponse<string>> SaveLastPasswordChangeDateTime(int userId)
        {
            var response = new BaseServiceResponse<string>("");

            var result = await db.SaveLastPasswordChangeDateTime(userId);
            if (result == 0)
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Last password change date/time could not be updated";
            }

            return response;
        }
    }
}
