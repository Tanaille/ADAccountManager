using ADAccountManager.Models;

namespace ADAccountManager.Utilities.ConfigService
{
    internal interface IConfigService
    {
        Config ReadConfig(string configFilePath);
    }
}
