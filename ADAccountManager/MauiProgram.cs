using Microsoft.Extensions.Logging;
using ADAccountManager.Utilities.Services;
using ADAccountManager.Views.HomePage;
using ADAccountManager.Views.SettingsPage;
using ADAccountManager.Views.UserCreationView;

namespace ADAccountManager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		// Register services
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<HomePage>();
		builder.Services.AddSingleton<SettingsView>();
		builder.Services.AddSingleton<SingleUserCreationView>();
		builder.Services.AddSingleton<MultiUserCreationView>();
		builder.Services.AddSingleton<IActiveDirectoryService, ActiveDirectoryService>();
		builder.Services.AddSingleton<IConfigService, ConfigService>();

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
