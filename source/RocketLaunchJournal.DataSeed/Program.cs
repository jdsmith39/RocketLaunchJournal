using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RocketLaunchJournal.DataSeed.Seeders;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.Enums;
using System;
using System.Threading.Tasks;

namespace RocketLaunchJournal.DataSeed
{
    class Program
    {
        async static Task Main(string[] args)
        {
            System.Console.WriteLine("Seeder Started...");
            System.Console.WriteLine("Type: ");
            System.Console.WriteLine(" 1 - \"enum\" to seed enum values.");
            System.Console.WriteLine(" 2 - \"rcu\" to seed roles and users data.");
            System.Console.WriteLine(" 3 - to seed rockets/launches.");

            var builder = new DbContextOptionsBuilder<DataContext>();

            string projectPath = AppDomain.CurrentDomain.BaseDirectory!.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configBuilder = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(typeof(Program).Assembly)
                .Build();

            builder.UseSqlServer(configBuilder.GetConnectionString("DefaultConnection"));

            var ups = new UserPermissionService();
            ups.Setup(new UserClaimBuilderTester());
            
            var context = new DataContext(ups, builder.Options, null);

            SeedBase? seeder = null;
            while (true)
            {
                var command = System.Console.ReadLine();
                
                switch (command.ToLower())
                {
                    case "1":
                    case "enum":
                        seeder = new SeedEnums(context);
                        await seeder.Seed();
                        System.Console.WriteLine("Seed complete");
                        break;
                    case "2":
                    case "rcu":
                        seeder = new SeedRolesUsers(context);
                        await seeder.Seed();
                        System.Console.WriteLine("Seed complete");
                        break;
                    case "3":
                        seeder = new SeedRocketsLaunches(context);
                        await seeder.Seed();
                        System.Console.WriteLine("Seed complete");
                        break;
                    default:
                        System.Console.WriteLine("bye");
                        return;
                }
            }
        }
    }
}
