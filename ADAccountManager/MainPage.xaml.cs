using ADAccountManager.Models;
using CsvHelper;
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

	private void OnCounterClicked(object sender, EventArgs e)
	{
		ADUser user = new ADUser(_context);
		user.CreateUsersFromCsvAsync(@"C:\Users\netadmin\OneDrive - Ferrum High School\Desktop\users.csv");
		//user.DeleteUser("riane.pot");
		//user.CreateUser("Riane", "Pot", "riane.pot", "ferrum.local");
		//user.Exists("riane.pot");

		//ADGroup group = new ADGroup(new PrincipalContext(ContextType.Domain, "ferrum.local"));
		//group.AddGroupMember(user.GetUser("riane.pot"), "StaffAccounts");
    }
}

