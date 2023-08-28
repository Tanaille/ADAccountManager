using ADAccountManager.Utilities.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Views.UserCreationView
{
    public class MultiUserCreationView
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IConfigService _configService;

        private Button csvSelector;
        private Button createUser;

        public Grid AddElements()
        {
            Grid multiUserCreationGrid = new Grid
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
                }
            };

            // Define the multi user input elements
            csvSelector = new Button { Text = "Select CSV File" };
            createUser = new Button { Text = "Create User" };

            multiUserCreationGrid.Add(csvSelector, 0, 0);
            multiUserCreationGrid.Add(createUser, 0, 1);

            multiUserCreationGrid.SetColumnSpan(csvSelector, 3);
            multiUserCreationGrid.SetColumnSpan(createUser, 3);

            return multiUserCreationGrid;
        }
    }
}
