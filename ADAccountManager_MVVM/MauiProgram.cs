using ADAccountManager_MVVM.Services.ConfigurationServices;
using Microsoft.Extensions.Logging;

namespace ADAccountManager_MVVM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Register services for DI
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<MainPage>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}