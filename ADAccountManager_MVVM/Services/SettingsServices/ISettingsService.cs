namespace ADAccountManager_MVVM.Services.ConfigurationServices
{
    public interface ISettingsService
    {
        public Models.Settings Settings { get; }
        public Models.Settings ReadSettings();
        public void SaveSettings(Models.Settings settings);
        public string GetSettingsFilePath();
    }
}
