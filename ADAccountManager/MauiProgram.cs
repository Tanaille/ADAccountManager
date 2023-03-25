using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.DirectoryServices.AccountManagement;
using ADAccountManager.Utilities.UserService;
using ADAccountManager.Utilities.GroupService;
using ADAccountManager.Models;

namespace ADAccountManager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
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
