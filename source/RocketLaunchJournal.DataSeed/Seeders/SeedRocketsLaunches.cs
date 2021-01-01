using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Model;
using RocketLaunchJournal.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RocketLaunchJournal.DataSeed.Seeders
{
    public class SeedRocketsLaunches : SeedBase
    {
        public SeedRocketsLaunches(DataContext context) : base(context) { }

        public override async System.Threading.Tasks.Task Seed()
        {
            await RocketsAndLaunches();
        }

        private async System.Threading.Tasks.Task RocketsAndLaunches()
        {
            var timestamp = DateTime.UtcNow;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "RocketLaunchJournal.DataSeed.SeedData.rockets.csv";
            _context.EnableTriggers = false;
            var rockets = new List<Rocket>();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName)!)
            using (StreamReader reader = new StreamReader(stream))
            {
                var line = reader.ReadLine(); // skip header row
                line = reader.ReadLine();
                while (line != null)
                {
                    var lineArray = line.ParseCSVLine();

                    var dbo = new Rocket()
                    {
                        AuditFields = new Model.OwnedTypes.AuditFields(0, timestamp),
                        UserId = 1,
                    };
                    dbo.RocketId = int.Parse(lineArray[0].Trim('"').Trim());
                    var isActive = bool.Parse(lineArray[1].Trim('"').Trim());
                    if (!isActive)
                    {
                        dbo.AuditFields.InactivatedById = 0;
                        dbo.AuditFields.InactiveDateTime = timestamp;
                    }
                    dbo.Name = lineArray[2].Trim('"').Trim();
                    if (!string.IsNullOrWhiteSpace(lineArray[4]))
                        dbo.Weight = decimal.Parse(lineArray[4].Trim('"').Trim());

                    rockets.Add(dbo);
                    line = reader.ReadLine();
                }

                
                foreach (var item in rockets)
                {
                    var dbObj = await _context.Rockets.FirstOrDefaultAsync(w => w.RocketId == item.RocketId);
                    if (dbObj == null)
                    {
                        dbObj = new Rocket()
                        {
                            RocketId = item.RocketId,
                            UserId = item.UserId
                        };
                        _context.Rockets.Add(dbObj);
                    }

                    dbObj.Name = item.Name;
                    dbObj.Weight = item.Weight;
                    dbObj.AuditFields = item.AuditFields;
                }
            }
            _context.Database.OpenConnection();
            _context.Database.ExecuteSqlRaw("Set Identity_Insert dbo.Rockets ON;");
            await _context.SaveChangesAsync();
            _context.Database.ExecuteSqlRaw("Set Identity_Insert dbo.Rockets OFF;");

            resourceName = "RocketLaunchJournal.DataSeed.SeedData.launches.csv";
            var launches = new List<Launch>();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName)!)
            using (StreamReader reader = new StreamReader(stream))
            {
                var line = reader.ReadLine(); // skip header row
                line = reader.ReadLine();
                while (line != null)
                {
                    var dbo = new Launch();
                    var lineArray = line.ParseCSVLine();
                    dbo.LaunchId = int.Parse(lineArray[0]);
                    dbo.RocketId = int.Parse(lineArray[1]);
                    dbo.LaunchNumber = int.Parse(lineArray[2]);
                    dbo.Date = DateTime.Parse(lineArray[3]);
                    dbo.Motors = lineArray[4].Trim('"').Trim();
                    if (!string.IsNullOrWhiteSpace(lineArray[6]))
                        dbo.Altitude = decimal.Parse(lineArray[6]);
                    if (!string.IsNullOrWhiteSpace(lineArray[7]))
                        dbo.TopSpeed = decimal.Parse(lineArray[7]);
                    if (!string.IsNullOrWhiteSpace(lineArray[8]))
                        dbo.BurnTime = decimal.Parse(lineArray[8]);
                    if (!string.IsNullOrWhiteSpace(lineArray[9]))
                        dbo.PeakAcceleration = decimal.Parse(lineArray[9]);
                    if (!string.IsNullOrWhiteSpace(lineArray[10]))
                        dbo.AverageAcceleration = decimal.Parse(lineArray[10]);
                    if (!string.IsNullOrWhiteSpace(lineArray[11]))
                        dbo.CoastToApogeeTime = decimal.Parse(lineArray[11]);
                    if (!string.IsNullOrWhiteSpace(lineArray[12]))
                        dbo.ApogeeToEjectionTime = decimal.Parse(lineArray[12]);
                    if (!string.IsNullOrWhiteSpace(lineArray[13]))
                        dbo.EjectionAltitude = decimal.Parse(lineArray[13]);
                    if (!string.IsNullOrWhiteSpace(lineArray[14]))
                        dbo.DescentSpeed = decimal.Parse(lineArray[14]);
                    if (!string.IsNullOrWhiteSpace(lineArray[15]))
                        dbo.Duration = decimal.Parse(lineArray[15]);
                    if (!string.IsNullOrWhiteSpace(lineArray[16]))
                        dbo.Note = lineArray[16].Trim('"').Trim();

                    launches.Add(dbo);
                    line = reader.ReadLine();
                }

                foreach (var item in launches)
                {
                    var dbObj = await _context.Launches.FirstOrDefaultAsync(w => w.RocketId == item.RocketId && w.LaunchNumber == item.LaunchNumber);
                    if (dbObj == null)
                    {
                        dbObj = new Launch()
                        {
                            AuditFields = new Model.OwnedTypes.AuditFields(0, timestamp),
                            LaunchId = item.LaunchId,
                            RocketId = item.RocketId,
                            LaunchNumber = item.LaunchNumber
                        };
                        _context.Launches.Add(dbObj);
                    }

                    dbObj.Date = item.Date;
                    dbObj.Motors = item.Motors;
                    dbObj.Altitude = item.Altitude;
                    dbObj.TopSpeed = item.TopSpeed;
                    dbObj.BurnTime = item.BurnTime;
                    dbObj.PeakAcceleration = item.PeakAcceleration;
                    dbObj.AverageAcceleration = item.AverageAcceleration;
                    dbObj.CoastToApogeeTime = item.CoastToApogeeTime;
                    dbObj.ApogeeToEjectionTime = item.ApogeeToEjectionTime;
                    dbObj.EjectionAltitude = item.EjectionAltitude;
                    dbObj.DescentSpeed = item.DescentSpeed;
                    dbObj.Duration = item.Duration;
                    dbObj.Note = item.Note;
                }
            }

            await _context.Database.ExecuteSqlRawAsync("Set Identity_Insert dbo.Launches ON;");
            await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlRawAsync("Set Identity_Insert dbo.Launches OFF;");

            _context.EnableTriggers = true;
        }
    }
}
