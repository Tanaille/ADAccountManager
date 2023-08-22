using System.Text.Json;
using ADAccountManager.Models;
using ADAccountManager.Views;

namespace ADAccountManager.Utilities.ConfigService
{
    internal static class ConfigService
    {
        public static Config ReadConfig(string configFilePath)
        {
            using StreamReader reader = new StreamReader(configFilePath);

            string json = reader.ReadToEnd();
            Config config = JsonSerializer.Deserialize<Config>(json);

            return config;
        }

        public static void WriteConfig(SettingsPage settingsPage, string configFilePath)
        {
            Config config = new Config
            {
                DomainName = settingsPage.DomainNameEntry.Text,
                MailDomain = settingsPage.MailDomainEntry.Text,
                DomainUser = settingsPage.DomainUserEntry.Text,
                DomainPassword = settingsPage.DomainPasswordEntry.Text,
                DefaultDomainOU = settingsPage.DefaultDomainOuEntry.Text
            };

            string json = JsonSerializer.Serialize(config);

            File.WriteAllText(configFilePath, json);
        }
    }
}
