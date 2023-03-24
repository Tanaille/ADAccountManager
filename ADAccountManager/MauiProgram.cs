using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.DirectoryServices.AccountManagement;
using ADAccountManager.Utilities.UserService;
using ADAccountManager.Utilities.GroupService;

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

		builder.Services.AddSingleton<PrincipalContext>(ctx => new PrincipalContext(ContextType.Domain, "ferrum.local"));
		builder.Services.AddScoped<IUserService, UserService>();
		builder.Services.AddScoped<IGroupService, GroupService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
