using Microsoft.Extensions.Logging;
using System;

namespace SampleConsoleApp
{
    public class Program
    {
        private static DateTime procDt;
        private static IBatService service;
        private static ILogger<Program> logger;

        public static void Main(string[] args)
        {
            procDt = DateTime.Now;
            Console.WriteLine($"[START] - {procDt:yyyy/MM/dd HH:mm:ss}");

            Setup();
            Console.WriteLine("Hello World!");

            var endDt = DateTime.Now;
            Console.WriteLine($"[ END ] - {endDt:yyyy/MM/dd HH:mm:ss} ({endDt.Subtract(procDt).TotalSeconds} sec)");
        }

        public static void Setup(IBatService batService, ILogger<Program> batLogger)
        {
            service = batService;
            logger = batLogger;
        }

        public static void Setup()
        {
            if (service == null)
            {
                var logFactory = LoggerFactory.Create(builder => { });
                Setup(new BatService(logFactory.CreateLogger<BatService>()), logFactory.CreateLogger<Program>());
            }
        }
    }
}
