using ADAccountManager.Models;
using ADAccountManager.Utilities;
using CsvHelper;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Globalization;

namespace ADAccountManager;

public partial class MainPage : ContentPage
{
	//private readonly PrincipalContext _context = new PrincipalContext(ContextType.Domain, "ferrum.local", "OU=Dev,OU=UserAccounts,DC=ferrum,DC=local");

	public MainPage()
	{
		InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
	{


        //var helper = new ADDirectoryHelper();
        //var result = await helper.GetAllOUsInDomainAsync("ferrum.local");
        
    }

    private void TestClick(object sender, EventArgs e)
    {

    }
}

