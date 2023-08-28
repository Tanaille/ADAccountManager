using ADAccountManager.Models;
using ADAccountManager.Utilities.Services;
using ADAccountManager.Utilities.UserService;
using ADAccountManager.Views.SettingsPage;
using ADAccountManager.Views.UserCreationView;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADAccountManager.Views.HomePage
{
    public class HomePage : ContentPage
    {
        private static IActiveDirectoryService _activeDirectoryService;
        private static IConfigService _configService;

        private StackLayout userCreationStackLayout;

        public HomePage(IActiveDirectoryService activeDirectoryService, IConfigService configService)
        {
            _activeDirectoryService = activeDirectoryService;
            _configService = configService;

            StackLayout homePageStackLayout = new StackLayout();

            // Define the HomePage GridLayout
            Grid homePageGridLayout = new Grid
            {
                Margin = 20,
                ColumnSpacing = 10,
                RowSpacing = 5,

                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Auto },
                    new ColumnDefinition {Width = GridLength.Star },
                    new ColumnDefinition {Width = 30 },
                },

                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                }
            };


            // Define the user creation mode selection radio buttons
            RadioButton singleUserSelectionRadioButton = new RadioButton
            {
                GroupName = "UserSelectionRadioButtonGroup",
                Content = "Single User Creation",
                IsChecked = true
            };

            RadioButton multiUserSelectionRadioButton = new RadioButton
            {
                GroupName = "UserSelectionRadioButtonGroup",
                Content = "Multi User Creation",
                IsChecked = false
            };

            homePageGridLayout.Add(singleUserSelectionRadioButton, 0, 0);
            homePageGridLayout.Add(multiUserSelectionRadioButton, 0, 1);

            ImageButton settingsIcon = new ImageButton
            {
                Source = ImageSource.FromFile("settings.png")
            };

            settingsIcon.Clicked += SettingsIcon_Clicked;

            homePageGridLayout.Add(settingsIcon, 2, 0);

            homePageStackLayout.Add(homePageGridLayout);


            userCreationStackLayout = new StackLayout();
            homePageStackLayout.Add(userCreationStackLayout);

            SingleUserCreationView singleUserCreation = new SingleUserCreationView();
            userCreationStackLayout.Add(singleUserCreation.AddElements());


            singleUserSelectionRadioButton.CheckedChanged += SingleUserSelectionRadioButton_CheckedChanged;
            multiUserSelectionRadioButton.CheckedChanged += MultiUserSelectionRadioButton_CheckedChanged;


            Content = homePageStackLayout;
        }

        private void SettingsIcon_Clicked(object sender, EventArgs e)
        {
            userCreationStackLayout.Clear();
            SettingsView settingsView = new SettingsView();
            userCreationStackLayout.Add(settingsView.AddElements(_configService));
        }

        private void SingleUserSelectionRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            userCreationStackLayout.Clear();
            SingleUserCreationView singleUserCreation = new SingleUserCreationView();
            userCreationStackLayout.Add(singleUserCreation.AddElements());
        }

        private void MultiUserSelectionRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            userCreationStackLayout.Clear();
            MultiUserCreationView multiUserCreation = new MultiUserCreationView();
            userCreationStackLayout.Add(multiUserCreation.AddElements());
        }
    }
}
