using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Model;
using RocketLaunchJournal.Model.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.DataSeed.Seeders
{
    public class SeedEnums : SeedBase
    {
        public SeedEnums(DataContext context) : base(context) { }

        public override async System.Threading.Tasks.Task Seed()
        {
            await LogTypes();
        }

        private async System.Threading.Tasks.Task LogTypes()
        {
            var enumValues = LogTypeEnum.Address.GetList();
            var values = await _context.LogTypes.ToListAsync();
            foreach (var item in enumValues)
            {
                var dbObj = values.FirstOrDefault(w => w.LogTypeId == item);
                if (dbObj == null)
                {
                    dbObj = new LogType()
                    {
                        LogTypeId = item
                    };
                    _context.LogTypes.Add(dbObj);
                }

                var displayName = item.GetDisplayName();
                if (dbObj.Name != displayName)
                    dbObj.Name = displayName;
            }

            await _context.SaveChangesAsync();
        }
    }
}
