using ADAccountManager.Models;
using ADAccountManager.Utilities;
using ADAccountManager.Utilities.ConfigService;
using ADAccountManager.Utilities.UserService;
using CsvHelper;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Drawing.Text;
using System.Globalization;

namespace ADAccountManager;

public partial class MainPage : ContentPage
{
    private readonly IConfigService _configService = new ConfigService();
    private readonly PrincipalContext _context;

	public MainPage()
	{
		InitializeComponent();

        var config = _configService.ReadConfig("C:\\Users\\Tanaille\\source\\repos\\Tanaille\\ADAccountManager\\ADAccountManager\\Resources\\Config\\appsettings.json");
        _context = new PrincipalContext(ContextType.Domain, config.DomainName, config.DefaultDomainOU, config.DomainUser, config.DomainPassword);
    }

    private async void OnCounterClicked(object sender, EventArgs e)
	{
        User user = new User{
            FirstName = "Test",
            LastName = "User",
            Domain = "ferrum.local",
            UserPrincipalName = "test.user"
        };


        //IUserCreator userCreator = new UserCreator(_context);
        //await userCreator.CreateUserAsync(user);

        //IUserFinder userFinder = new UserFinder(_context);
        //await userFinder.GetUserAsync("test.user@ferrum.local");

        //IUserDeleter userDeleter = new UserDeleter(_context, userFinder);
        //await userDeleter.DeleteUserAsync("test.user@ferrum.local");
    }

    private void TestClick(object sender, EventArgs e)
    {

    }
}

