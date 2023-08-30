using System.Text.Json;

namespace ADAccountManager_MVVM.Services.ConfigurationServices
{
    public class SettingsService : ISettingsService
    {
        public Models.Settings Settings { get; }

        public SettingsService()
        {
            Settings = ReadSettings();
        }

        public Models.Settings ReadSettings()
        {
            using StreamReader reader = new StreamReader(GetSettingsFilePath());

            string json = reader.ReadToEnd();
            Models.Settings settings = JsonSerializer.Deserialize<Models.Settings>(json);

            return settings;
        }

        public void SaveSettings(Models.Settings settings)
        {
            File.WriteAllText(GetSettingsFilePath(), JsonSerializer.Serialize(settings));
        }

        public string GetSettingsFilePath()
        {
            // Get the path to the Settings.json file
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = "Resources\\Settings\\Settings.json";
            return Path.Combine(basePath, relativePath);
        }
    }
}
