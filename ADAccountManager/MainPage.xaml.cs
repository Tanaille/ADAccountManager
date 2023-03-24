﻿using ADAccountManager.Models;
using ADAccountManager.Utilities;
using ADAccountManager.Utilities.UserService;
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
        //User user = new User{
        //    FirstName = "Test",
        //    LastName = "User",
        //    Domain = "ferrum.local",
        //    UserPrincipalName = "test.user"
        //};
        
        
        //userService.GetUserAsync("test.user@ferrum.local", _context);


    }

    private void TestClick(object sender, EventArgs e)
    {

    }
}

