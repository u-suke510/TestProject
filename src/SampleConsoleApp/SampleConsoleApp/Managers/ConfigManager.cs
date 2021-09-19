using Microsoft.Extensions.Configuration;

namespace SampleConsoleApp.Managers
{
    public interface IConfigManager
    {
        string ConnString
        {
            get;
        }

        string CsvPath
        {
            get;
        }
    }

    public class ConfigManager : IConfigManager
    {
        private static readonly string KeyConnString = "ConnectionString";
        private static readonly string KeyCsvPath = "CsvPath";

        public string CsvPath
        {
            get;
            set;
        }

        public string ConnString
        {
            get;
            set;
        }

        public ConfigManager(IConfiguration config)
        {
            var values = config.GetSection("Values");

            ConnString = config.GetConnectionString(KeyConnString);
            CsvPath = values[KeyCsvPath];
        }
    }
}
