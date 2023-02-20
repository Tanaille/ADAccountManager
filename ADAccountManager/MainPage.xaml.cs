using ADAccountManager.Models;
using CsvHelper;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Globalization;

namespace ADAccountManager;

public partial class MainPage : ContentPage
{
	private readonly PrincipalContext _context = new PrincipalContext(ContextType.Domain, "ferrum.local", "OU=Dev,OU=UserAccounts,DC=ferrum,DC=local");

	public MainPage()
	{
		InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
	{
		ADUser user = new ADUser(_context);
        //await user.CreateUsersFromCsvAsync(@"C:\Users\netadmin\OneDrive - Ferrum High School\Desktop\users.csv");
        //user.DeleteUser("riane.pot");
        //await user.CreateUserAsync("Lindelwa", "Dlamini", "lindelwa.dlamini", "ferrum.local");
        //user.Exists("riane.pot");
        //var s = ConfigurationManager.AppSettings.Get("Domain");
        //ADGroup group = new ADGroup(new PrincipalContext(ContextType.Domain, "ferrum.local"));
        //group.AddGroupMember(user.GetUser("riane.pot"), "StaffAccounts");
        //Preferences.Set("ferrum", "local");
        //await DisplayAlert("", Preferences.Get("ferrum", "default"), "OK");
        List<string> groups = new List<string>();
        groups.Add("StaffAccounts");
        await user.CreateUserWithGroupsAsync("Riane", "Pot", "riane.pot", "ferrum.local", groups);
    }
}

