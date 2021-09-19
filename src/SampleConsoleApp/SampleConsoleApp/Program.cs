using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleConsoleApp.Managers;
using System;
using System.Linq;

namespace SampleConsoleApp
{
    public class Program
    {
        private static DateTime procDt;
        private static IBatService service;
        private static IConfigManager conf;

        public static void Main(string[] args)
        {
            DateTime endDt;
            procDt = DateTime.Now;
            Console.WriteLine($"[START] - {procDt:yyyy/MM/dd HH:mm:ss}");
            var isTruncate = false;
            if (args.Any())
            {
                // args[0] is isTruncate.
                var logArgs =args.Select((val, idx) => new { val, idx })
                    .Select(x => $"args[{x.idx}]:{x.val}");
                Console.WriteLine($"          {string.Join(", ", logArgs)}");
                bool.TryParse(args[0], out isTruncate);
            }
            else
            {
                Console.WriteLine($"          args: none.");
            }

            // setup bat service
            Setup();
            service.EnsureTable(isTruncate);

            // parse csv
            var data = service.ParseCsv(conf.CsvPath);
            if (data == null)
            {
                endDt = DateTime.Now;
                Console.WriteLine($"[ END ] - {endDt:yyyy/MM/dd HH:mm:ss} ({endDt.Subtract(procDt).TotalSeconds} sec)");
                return;
            }

            // add users
            foreach (var cols in data)
            {
                // exist user
                if (service.ExistUser(cols[1]))
                {
                    Console.WriteLine($"{cols[1]} Exist User.");
                    continue;
                }
                Console.WriteLine($"[0]:{cols[0]}, [1]:{cols[1]}, [2]:{cols[2]}");
                service.AddUser(cols);
            }

            endDt = DateTime.Now;
            Console.WriteLine($"[ END ] - {endDt:yyyy/MM/dd HH:mm:ss} ({endDt.Subtract(procDt).TotalSeconds} sec)");
        }

        public static void Setup()
        {
            if (service == null)
            {
                var confBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true);
                var confManager = new ConfigManager(confBuilder.Build());

                var services = new ServiceCollection();
                services.AddDbContext<AppDbContext>(option => { option.UseSqlServer(confManager.ConnString); });
                services.AddLogging(builder => { builder.AddConsole(); });
                services.AddSingleton<IBatService, BatService>();
                var provider = services.BuildServiceProvider();
                provider.GetService<IBatService>();

                Setup(provider.GetService<IBatService>(), confManager);
            }
        }

        public static void Setup(IBatService batService, IConfigManager confManager)
        {
            service = batService;
            conf = confManager;
        }
    }
}
