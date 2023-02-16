using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

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
		//user.DeleteUser("riane.pot");
		//user.CreateUser("Riane", "Pot", "riane.pot", "ferrum.local");
		//user.Exists("riane.pot");
    }
}

