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
        private readonly string _configFilePath;

        public ConfigService()
        {
            // Retrieve and read the config file
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = "Resources\\Config\\appsettings.json";
            _configFilePath = Path.Combine(basePath, relativePath);


            using StreamReader reader = new StreamReader(_configFilePath);

            string json = reader.ReadToEnd();
            _config = JsonSerializer.Deserialize<Config>(json);
        }

        public Config GetConfig()
        {
            return _config;
        }

        public void SetConfig(Config config)
        {
            File.WriteAllText(_configFilePath, JsonSerializer.Serialize(config));
        }
    }
}
