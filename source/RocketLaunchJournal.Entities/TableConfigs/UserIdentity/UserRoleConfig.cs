using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketLaunchJournal.Entities.TableConfigs.UserIdentity
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.RoleId });
        }
    }
}
