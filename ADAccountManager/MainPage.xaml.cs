using ADAccountManager.Models;
using ADAccountManager.Utilities;
using ADAccountManager.Utilities.ConfigService;
using ADAccountManager.Utilities.CsvOperations;
using ADAccountManager.Utilities.CsvService;
using ADAccountManager.Utilities.FileOperations;
using ADAccountManager.Utilities.GroupService;
using ADAccountManager.Utilities.UserService;
using ADAccountManager.Utilities.ViewModelOperations;
using ADAccountManager.Utilities.WindowLayoutOperations;
using System.Collections.ObjectModel;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace ADAccountManager;

public partial class MainPage : ContentPage
{
    private readonly IConfigService _configService = new ConfigService();
    private readonly PrincipalContext _context;
    private readonly Config _config;
    private FileResult _fileResult;

    public MainPage()
    {
        InitializeComponent();

        _config = _configService.ReadConfig("C:\\Users\\netadmin\\source\\repos\\ADAccountManager\\ADAccountManager\\Resources\\Config\\appsettings.json");
        _context = new PrincipalContext(ContextType.Domain, _config.DomainName, _config.DefaultDomainOU, _config.DomainUser, _config.DomainPassword);
    }

    // Displays the selected user creation method (single user or multi user creation).
    private void UserCreationMethodSet_CheckedChanged(object sender, EventArgs e)
    {
        if (MultipleUserCreationRadioButton.IsChecked)
        {
            MultipleUserCreationStackLayout.IsVisible = true;
            SingleUserCreationStackLayout.IsVisible = false;
        }

        else
        {
            MultipleUserCreationStackLayout.IsVisible = false;
            SingleUserCreationStackLayout.IsVisible = true;
        }
    }

    // Handles .csv file selection and stores the result in _fileResult.
    private async void CsvPicker_Clicked(object sender, EventArgs e)
    {
        // Define a custom file picker file type for .csv files.
        FilePickerFileType csvFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                            { DevicePlatform.WinUI, new[] { ".csv" } }, // CSV file extension.
                });

        _fileResult = await FileOperations.PickFile(new PickOptions
        {
            PickerTitle = "Select the CSV file",
            FileTypes = csvFileType
        });
    }

    // Create a single user account using the details entered in the main form.
    private async void CreateSingleUserButton_Clicked(object sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text;
        string lastName = LastNameEntry.Text;
        string mobile = MobileEntry.Text;
        string userPrincipalName = firstName.ToLower() + "." + lastName.ToLower();
        userPrincipalName = Regex.Replace(userPrincipalName, @"\s+", ""); // Strip whitespace from the UPN.
        string mailDomain = _config.MailDomain;

        IUserPrincipalCreator userPrincipalCreator = new UserPrincipalCreator(_context);
        ADUser user = new ADUser()
        {
            FirstName = firstName,
            LastName = lastName,
            UserPrincipalName = userPrincipalName,
            Domain = mailDomain,
            MobilePhone = mobile,
        };

        try
        {
            var result = await userPrincipalCreator.CreateUserPrincipalAsync(user);

            if (result)
                await DisplayAlert("Created users", "The following user principals have been created:\n\n" + user.FirstName + " " + user.LastName, "OK");

            else
                await DisplayAlert("User creation failed", "The following user principals could not be created:\n\n" + user.FirstName + " " + user.LastName, "OK");
        }
        catch (Exception ex)
        {
            object userMessage = ex.Data["UserMessage"];
            await DisplayAlert("Error: " + ex.Message, userMessage.ToString(), "OK");
        }
    }

    // Create multiple user accounts simultaneously by using data stored in a pre-formatted CSV file.
    private async void CreateMultipleUsersButton_Clicked(object sender, EventArgs e)
    {
        IUserPrincipalCreator userPrincipalCreator = new UserPrincipalCreator(_context);
        ICsvService csvService = new CsvService();
        ICsvOperations csvOperations = new CsvOperations(csvService, userPrincipalCreator, _context);

        try
        {
            var result = await csvOperations.CreateUserPrincipalsFromCsvAsync(_fileResult.FullPath);

            if (result.NotCreatedUserPrincipals.Count > 0)
            {
                string principals = string.Empty;

                foreach (var userPrincipal in result.NotCreatedUserPrincipals)
                    principals += (userPrincipal.UserPrincipalName + "\n");

                await DisplayAlert("User creation failed", "The following user principals could not be created:\n\n" + principals, "OK");
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
}