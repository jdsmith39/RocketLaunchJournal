using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace RocketLaunchJournal.Infrastructure.UserIdentity
{
    public class UserPermissionService
    {
        public UserPermissionService()
        {
        }

        public bool IsSetup { get; private set; }

        public string? FirstLastName
        {
            get
            {
                if (UserClaimModel != null)
                    return $"{UserClaimModel.FirstName} {UserClaimModel.LastName}";
                return null;
            }
        }

        public string? LastFirstName
        {
            get
            {
                if (UserClaimModel != null)
                    return $"{UserClaimModel.LastName}, {UserClaimModel.FirstName}";
                return null;
            }
        }

        public UserClaimModel UserClaimModel { get; set; } = default!;

        public UserPolicies? UserPolicies { get { return UserClaimModel?.UserPolicies; } }

        public void Setup(UserClaimModel ucm)
        {
            UserClaimModel = ucm;
            IsSetup = true;
        }
    }
}
