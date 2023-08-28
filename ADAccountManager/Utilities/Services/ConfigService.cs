using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ADAccountManager.Models;

namespace ADAccountManager.Utilities.Services
{
    public class ConfigService : IConfigService
    {
        private readonly Config _config;

        public ConfigService()
        {
            // Retrieve and read the config file
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = "Resources\\Config\\appsettings.json";
            string configFilePath = Path.Combine(basePath, relativePath);


            using StreamReader reader = new StreamReader(configFilePath);

            string json = reader.ReadToEnd();
            _config = JsonSerializer.Deserialize<Config>(json);
        }

        public Config GetConfig()
        {
            return _config;
        }
    }
}
