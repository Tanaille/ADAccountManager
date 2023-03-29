using ADAccountManager.Models;
using ADAccountManager.Utilities;
using ADAccountManager.Utilities.ConfigService;
using ADAccountManager.Utilities.CsvOperations;
using ADAccountManager.Utilities.CsvService;
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

        var config = _configService.ReadConfig("C:\\Users\\netadmin\\source\\repos\\ADAccountManager\\ADAccountManager\\Resources\\Config\\appsettings.json");
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
        ICsvService csvService = new CsvService();
        ICsvOperations csvOperations = new CsvOperations(csvService, userPrincipalCreator, _context);

        try
        {
            var result = await csvOperations.CreateUserPrincipalsFromCsvAsync("C:\\Users\\netadmin\\OneDrive - Ferrum High School\\Desktop\\users.csv");
            
            if (result.NotCreatedUserPrincipals.Count > 0)
            {
                string principals = string.Empty;

                foreach (var userPrincipal in result.NotCreatedUserPrincipals)
                    principals += (userPrincipal.UserPrincipalName + "\n");

                await DisplayAlert("Not created users", "The following user principals have not been created:\n\n" + principals, "OK");
            }

            if (result.CreatedUserPrincipals.Count > 0)
            {
                string principals = string.Empty;

                foreach (var userPrincipal in result.CreatedUserPrincipals)
                    principals += (userPrincipal.UserPrincipalName + "\n");

                await DisplayAlert("Created users", "The following user principals have been created:\n\n" + principals, "OK");
            }
        }
        catch (Exception ex)
        {
            object userMessage = ex.Data["UserMessage"];
            await DisplayAlert("Error: " + ex.Message, userMessage.ToString(), "OK");
        }
    }

    private void TestClick(object sender, EventArgs e)
    {

    }
}

