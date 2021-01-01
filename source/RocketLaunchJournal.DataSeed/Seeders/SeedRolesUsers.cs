using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Model;
using RocketLaunchJournal.Model.OwnedTypes;
using RocketLaunchJournal.Model.SerializedObjects;
using RocketLaunchJournal.Model.UserIdentity;

namespace RocketLaunchJournal.DataSeed.Seeders
{
    public class SeedRolesUsers : SeedBase
    {
        private List<Role> _roles = new List<Role>();

        public SeedRolesUsers(DataContext context) : base(context) { }

        public override async System.Threading.Tasks.Task Seed()
        {
            await Roles();
            await Users();
        }

        private async System.Threading.Tasks.Task Roles()
        {
            _roles.Add(new Role() { RoleId = 1, Name = Role.Admin, NormalizedName = Role.Admin.ToLower(), Type = Role.RoleTypeGeneral, Level = 1, });

            _roles[0].Data = new RoleData() { GrantableRoleIds = _roles.Select(s => s.RoleId).ToList() }.SerializeJson();

            foreach (var item in _roles)
            {
                var dbObj = await _context.Roles.FirstOrDefaultAsync(w => w.RoleId == item.RoleId);
                if (dbObj == null)
                {
                    dbObj = new Role()
                    {
                        RoleId = item.RoleId
                    };
                    _context.Roles.Add(dbObj);
                }

                dbObj.Level = item.Level;
                dbObj.Name = item.Name;
                dbObj.NormalizedName = item.NormalizedName;
                dbObj.Type = item.Type;
                dbObj.Data = item.Data;
            }

            await _context.SaveChangesAsync();
        }

        private async System.Threading.Tasks.Task Users()
        {
            // role must be cloned for each use
            var adminRole = _roles.Where(w => w.Name == Role.Admin).Select(s => new UserRole() { RoleId = s.RoleId }).First();
            var users = new List<User>();

            users.Add(new User()
            {
                Email = "test@test.com",
                FirstName = "test",
                LastName = "test",
                PasswordHash = "ACMVM5I16Hmp1MN1VVvPoi3qDFSgQnx2ptICSzBMVkeElBJ6DB09lV4DFDKhu/nZQQ==",
                SecurityStamp = "09e6a71b-5a4f-4c54-b9b3-2fc54f114bb8",
                PhoneNumber = "1235554321",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                EmailConfirmed = true,
                IsLoginEnabled = true,
                AuditFields = new AuditFields(0, _timestamp),
            });

            users.Add(new User()
            {
                Email = "jdsmith39@gmail.com",
                FirstName = "Jeremy",
                LastName = "Smith",
                PasswordHash = "ACMVM5I16Hmp1MN1VVvPoi3qDFSgQnx2ptICSzBMVkeElBJ6DB09lV4DFDKhu/nZQQ==",
                SecurityStamp = "09e6a71b-5a4f-4c54-b9b3-2fc54f114bb8",
                PhoneNumber = "8102523799",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                EmailConfirmed = true,
                IsLoginEnabled = true,
                
                AuditFields = new AuditFields(0, _timestamp),
                UserRoles = new List<UserRole>() { adminRole.SerializeJson().DeserializeJson<UserRole>() },
            });

            for (int i = 0; i < users.Count; i++)
            {
                var item = users[i];
                item.NormalizedEmail = item.Email.ToUpper();

                var dbObj = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Email == item.Email);
                // re-adds if missing
                if (dbObj == null)
                    _context.Users.Add(item);
                else
                    users[i] = dbObj;
            }
            await _context.SaveChangesAsync();
        }
    }
}
