using ADAccountManager.Models;
using ADAccountManager.Utilities;
using ADAccountManager.Utilities.ConfigService;
using ADAccountManager.Utilities.GroupService;
using ADAccountManager.Utilities.UserService;
using System.DirectoryServices.AccountManagement;

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
        ADUser user = new ADUser{
            FirstName = "Test",
            LastName = "User",
            Domain = "ferrum.local",
            UserPrincipalName = "test.user"
        };

        Models.ADGroup group = new Models.ADGroup
        {
            Name = "TestGroup-FULL",
        };


        IUserPrincipalCreator userPrincipalCreator = new UserPrincipalCreator(_context);
        await userPrincipalCreator.CreateUserAsync(user);

        IUserPrincipalFinder userPrincipalFinder = new UserPrincipalFinder(_context);
        await userPrincipalFinder.GetUserAsync("test.user@ferrum.local");

        IUserPrincipalDeleter userPrincipalDeleter = new UserPrincipalDeleter(_context, userPrincipalFinder);
        await userPrincipalDeleter.DeleteUserAsync("test.user@ferrum.local");

        IGroupPrincipalCreator groupPrincipalCreator = new GroupPrincipalCreator(_context);
        await groupPrincipalCreator.CreateGroupPrincipalAsync(group);

        IGroupPrincipalFinder groupPrincipalFinder = new GroupPrincipalFinder(_context);
        await groupPrincipalFinder.GetGroupPrincipalAsync("TestGroup-FULL");

        IGroupPrincipalDeleter groupPrincipalDeleter = new GroupPrincipalDeleter(_context, groupPrincipalFinder);
        await groupPrincipalDeleter.DeleteGroupPrincipalAsync("TestGroup-FULL");
    }

    private void TestClick(object sender, EventArgs e)
    {

    }
}

