using IdentityModel;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Dtos.Users;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.SerializedObjects;
using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RocketLaunchJournal.Web.Shared.UserIdentity
{
    /// <summary>
    /// Builds the user claim model
    /// </summary>
    public class UserClaimBuilder : UserClaimModel
    {
        #region Claim Types

        public const string RoleDataClaimType = nameof(RoleData);
        public const string IPAddressClaimType = nameof(IpAddress);
        public const string OriginalSuffix = "Original";

        #endregion

        private ClaimsPrincipal? _userPrincipal;
        private IEnumerable<Claim>? _claims;

        /// <summary>
        /// Constructs the UserClaimBuilder using the claimsPrincipal
        /// </summary>
        /// <param name="claimsUser">System.Security.Claims.ClaimsPrincipal</param>
        public UserClaimBuilder(System.Security.Claims.ClaimsPrincipal? claimsUser)
        {
            _userPrincipal = claimsUser;
            if (claimsUser == null)
                return;
            
            _claims = _userPrincipal!.Claims;
            UserId = UserClaimInt(JwtClaimTypes.Subject);
            UserIdOriginal = UserClaimInt(JwtClaimTypes.Subject + OriginalSuffix);
            Email = UserClaimString(JwtClaimTypes.Email);
            FirstName = UserClaimString(JwtClaimTypes.GivenName);
            LastName = UserClaimString(JwtClaimTypes.FamilyName);
            IpAddress = UserClaimString(IPAddressClaimType);

            var roleDataString = UserClaimString(RoleDataClaimType);
            if (!string.IsNullOrEmpty(roleDataString))
                RoleData = roleDataString.DeserializeJson<RoleData>();

            // roles
            IsAdmin = _userPrincipal.IsInRole(Role.Admin);

            // policies
            SetupPolicies();
        }

        /// <summary>
        /// Generates additional claims for the passed in user for the SERVER
        /// These claims the ones that identity server DOES NOT already create
        /// 
        /// Can setup impersonation if optional user is passed in.
        /// </summary>
        /// <param name="user">user to impersonate</param>
        /// <param name="roles">user's roles</param>
        /// <param name="ipAddress">user's ip Address</param>
        /// <param name="userToImpersonate">user to impersonate</param>
        /// <returns>claims list</returns>
        public static List<Claim> GenerateClaimsServer(User user, IList<Role> roles, string ipAddress, User? userToImpersonate = null)
        {
            var claimsBasedOnUser = userToImpersonate ?? user;
            var role = roles.OrderBy(o => o.Level).FirstOrDefault();
            var claims = GenerateClaimsClient(user, userToImpersonate);

            if (ipAddress != null)
                claims.Add(new Claim(IPAddressClaimType, ipAddress, ClaimValueTypes.String));

            if (role != null && role.Data != null)
                claims.Add(new Claim(RoleDataClaimType, role.Data, ClaimValueTypes.String));

            return claims;
        }

        /// <summary>
        /// Generates additional claims for the passed in user for the CLIENT
        /// These claims the ones that identity server DOES NOT already create
        /// 
        /// Can setup impersonation if optional user is passed in.
        /// </summary>
        /// <param name="user">user to impersonate</param>
        /// <param name="userToImpersonate">user to impersonate</param>
        /// <returns>claims list</returns>
        public static List<Claim> GenerateClaimsClient(User user, User? userToImpersonate = null)
        {
            var claimsBasedOnUser = userToImpersonate ?? user;
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.Subject + OriginalSuffix, user.UserId.ToString(), ClaimValueTypes.Integer),
                new Claim(JwtClaimTypes.GivenName, claimsBasedOnUser.FirstName, ClaimValueTypes.String),
                new Claim(JwtClaimTypes.FamilyName, claimsBasedOnUser.LastName, ClaimValueTypes.String),
            };

            return claims;
        }

        /// <summary>
        /// Impersonate an user by setting claims accordingly
        /// </summary>
        /// <param name="claims">current list of claims</param>
        /// <param name="userDto">user to impersonate</param>
        /// <returns>claims list</returns>
        //public static List<Claim> ImpersonateUser(List<Claim> claims, UserDto userDto)
        //{
        //    var userIdClaim = claims.FirstOrDefault(w => w.Type == UserIdClaimType);
        //    var emailClaim = claims.FirstOrDefault(w => w.Type == JwtRegisteredClaimNames.Sub);
        //    var firstNameClaim = claims.FirstOrDefault(w => w.Type == JwtRegisteredClaimNames.GivenName);
        //    var lastNameClaim = claims.FirstOrDefault(w => w.Type == JwtRegisteredClaimNames.FamilyName);

        //    if (userIdClaim != null)
        //        claims.Remove(userIdClaim);
        //    if (emailClaim != null)
        //        claims.Remove(emailClaim);
        //    if (firstNameClaim != null)
        //        claims.Remove(firstNameClaim);
        //    if (lastNameClaim != null)
        //        claims.Remove(lastNameClaim);

        //    claims.Add(new Claim(UserIdClaimType, userDto.UserId.ToString(), ClaimValueTypes.Integer));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userDto.Email, ClaimValueTypes.String));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, userDto.FirstName, ClaimValueTypes.String));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.FamilyName, userDto.LastName, ClaimValueTypes.String));

        //    return claims;
        //}

        #region helpers

        private string UserClaimString(string claimToFind)
        {
            var claim = this._claims.FirstOrDefault(w => w.Type == claimToFind);
            if (claim == null)
                return "";
            if (claim.ValueType != ClaimValueTypes.String)
                throw new InvalidCastException($"User claim '{claimToFind}' is not of type String.");

            return claim.Value;
        }

        private int UserClaimInt(string claimToFind)
        {
            var claim = this._claims.FirstOrDefault(w => w.Type == claimToFind);
            if (claim == null)
                return -1;
            if (!int.TryParse(claim.Value, out int value))
                throw new InvalidCastException($"User claim '{claimToFind}' is not of type int.");

            return value;
        }

        private bool UserClaimBoolean(string claimToFind)
        {
            var claim = this._claims.FirstOrDefault(w => w.Type == claimToFind);
            if (claim == null)
                return false;
            if (claim.ValueType != ClaimValueTypes.Boolean)
                throw new InvalidCastException($"User claim '{claimToFind}' is not of type boolean.");
            return bool.Parse(claim.Value);
        }

        #endregion
    }
}

