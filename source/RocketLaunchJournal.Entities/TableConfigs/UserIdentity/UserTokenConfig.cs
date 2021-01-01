using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketLaunchJournal.Entities.TableConfigs.UserIdentity
{
    public class UserTokenConfig : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
        }
    }
}
