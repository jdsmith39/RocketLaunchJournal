using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketLaunchJournal.Entities.TableConfigs.UserIdentity
{
    public class UserLoginConfig : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasKey(r => new { r.LoginProvider, r.ProviderKey });
        }
    }
}
