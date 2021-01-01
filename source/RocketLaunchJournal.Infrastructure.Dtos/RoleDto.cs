using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketLaunchJournal.Infrastructure.Dtos
{
    public class RoleDto : RoleBase
    {
        public RoleDataDto? DataObj { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAdded { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is RoleDto role)
            {
                return RoleId == role.RoleId;
            }

            return false;
        }
    }
}
