using System.Text.Json;
using ADAccountManager.Models;

namespace ADAccountManager.Utilities.ConfigService
{
    internal class ConfigService : IConfigService
    {
        public Config ReadConfig(string configFilePath)
        {
            using StreamReader reader = new StreamReader(configFilePath);

            string json = reader.ReadToEnd();
            Config config = JsonSerializer.Deserialize<Config>(json);

            return config;
        }
    }
}
