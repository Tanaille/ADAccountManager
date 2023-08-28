using ADAccountManager.Utilities.Services;
using ADAccountManager.Utilities.WindowLayoutOperations;
using ADAccountManager.Views.HomePage;

namespace ADAccountManager;

public partial class MainPage : ContentPage
{
    private readonly HomePage _homePage;
    private readonly IConfigService _configService;

    public MainPage(HomePage homePage, IConfigService configService)
    {
        InitializeComponent();
        BindingContext = _homePage;

        _homePage = homePage;
        _configService = configService;

        Content = _homePage.Content;

        // Set the initial main window size
        WindowLayoutOperations.ChangeWindowSize(450, 480);
    }
}




































    //// Handles .csv file selection and stores the result in _fileResult.
    //private async void CsvPicker_Clicked(object sender, EventArgs e)
    //{
    //    //// Define a custom file picker file type for .csv files.
    //    //FilePickerFileType csvFileType = new FilePickerFileType(
    //    //        new Dictionary<DevicePlatform, IEnumerable<string>>
    //    //        {
    //    //                    { DevicePlatform.WinUI, new[] { ".csv" } }, // CSV file extension.
    //    //        });

    //    //_fileResult = await FileOperations.PickFile(new PickOptions
    //    //{
    //    //    PickerTitle = "Select the CSV file",
    //    //    FileTypes = csvFileType
    //    //});

    //    await Navigation.PushModalAsync(new Views.HomePage(DependencyService.Get<IActiveDirectoryService>()));
    //}


    //// Create multiple user accounts simultaneously by using data stored in a pre-formatted CSV file.
    //private async void CreateMultipleUsersButton_Clicked(object sender, EventArgs e)
    //{
    //    IUserPrincipalCreator userPrincipalCreator = new UserPrincipalCreator(DependencyService.Get<IActiveDirectoryService>());
    //    ICsvService csvService = new CsvService();
    //    ICsvOperations csvOperations = new CsvOperations(csvService, userPrincipalCreator, DependencyService.Get<IActiveDirectoryService>().GetPrincipalContext());

    //    try
    //    {
    //        var result = await csvOperations.CreateUserPrincipalsFromCsvAsync(_fileResult.FullPath);

    //        if (result.NotCreatedUserPrincipals.Count > 0)
    //        {
    //            string principals = string.Empty;

    //            foreach (var userPrincipal in result.NotCreatedUserPrincipals)
    //                principals += (userPrincipal.UserPrincipalName + "\n");

    //            await DisplayAlert("User creation failed", "The following user principals could not be created:\n\n" + principals, "OK");
    //        }

    //        if (result.CreatedUserPrincipals.Count > 0)
    //        {
    //            string principals = string.Empty;

    //            foreach (var userPrincipal in result.CreatedUserPrincipals)
    //                principals += (userPrincipal.UserPrincipalName + "\n");

    //            await DisplayAlert("Created users", "The following user principals have been created:\n\n" + principals, "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        object userMessage = ex.Data["UserMessage"];
    //        await DisplayAlert("Error: " + ex.Message, userMessage.ToString(), "OK");
    //    }
//    }
//}