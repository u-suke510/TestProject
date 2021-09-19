using Microsoft.Extensions.Logging;
using SampleConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SampleConsoleApp
{
    public interface IBatService
    {
        bool AddUser(string[] data);

        bool EnsureTable(bool isTblTruncate = false);

        bool ExistUser(string usrNm);

        List<string[]> ParseCsv(string path);
    }

    public class BatService : IBatService
    {
        private ILogger<BatService> logger;
        private AppDbContext context;

        public BatService(ILogger<BatService> logger, AppDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public bool AddUser(string[] data)
        {
            DateTime birthday;
            if (data == null || data.Length <= 0)
            {
                logger.LogWarning($"Data is Null.");
                return false;
            }
            else if (data.Length != 3)
            {
                logger.LogWarning($"Data Size {data.Length}.");
                return false;
            }
            else if (string.IsNullOrEmpty(data[0]))
            {
                logger.LogWarning($"DispName is Empty.");
                return false;
            }
            else if (string.IsNullOrEmpty(data[1]))
            {
                logger.LogWarning($"UserName is Empty.");
                return false;
            }
            else if (!DateTime.TryParse(data[2], out birthday))
            {
                logger.LogWarning($"Birthday is Format Error.");
                return false;
            }

            context.Add(new User {
                Id = Guid.NewGuid().ToString(),
                UserName = data[1],
                DispName = data[0],
                Email = data[1],
                Birthday = birthday
            });
            context.SaveChanges();

            return true;
        }

        public bool EnsureTable(bool isTblTruncate = false)
        {
            if (isTblTruncate)
            {
                context.ExecuteSqlRaw("TRUNCATE TABLE Users");
            }

            return context.Database.EnsureCreated();
        }

        public bool ExistUser(string usrNm)
        {
            return context.Users.Any(x => x.UserName == usrNm);
        }

        public List<string[]> ParseCsv(string path)
        {
            logger.LogInformation($"[Params] - path:{path}");

            if (!File.Exists(path))
            {
                logger.LogError($"File Not Found.({path})");
                return null;
            }

            var result = new List<string[]>();
            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                while (sr.Peek() > -1)
                {
                    var items = sr.ReadLine().Split(",");
                    result.Add(items);
                }
            }

            return result;
        }
    }
}
