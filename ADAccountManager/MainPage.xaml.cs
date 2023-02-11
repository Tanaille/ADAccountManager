using ADAccountManager.Models;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);

		ADUser user = new ADUser(new PrincipalContext(ContextType.Domain, "FERRUM", "OU=Dev,OU=UserAccounts,DC=ferrum,DC=local"));
		//bool tmp = user.SearchUser("test.user");
		//user.DeleteUser("test.user");
		user.CreateUser("Test", "User", "test.user", "ferrum.local");
	}
}

