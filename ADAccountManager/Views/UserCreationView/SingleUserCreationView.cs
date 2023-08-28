using ADAccountManager.Utilities.Services;
using ADAccountManager.Views.HomePage;

namespace ADAccountManager.Views.UserCreationView
{
    public class SingleUserCreationView
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IConfigService _configService;

        private Entry singleUserFirstNameEntry;
        private Entry singleUserLastNameEntry;
        private Entry singleUserMobileEntry;

        public Grid AddElements()
        {
            Grid singleUserCreationGrid = new Grid()
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
                }
            };

            // Define the single user info input elements
            Label singleUserFirstNameLabel = new Label { Text = "First name:", VerticalTextAlignment = TextAlignment.Center };
            Label singleUserLastNameLabel = new Label { Text = "Last name:", VerticalTextAlignment = TextAlignment.Center };
            Label singleUserMobileLabel = new Label { Text = "Mobile:", VerticalTextAlignment = TextAlignment.Center };

            singleUserFirstNameEntry = new Entry();
            singleUserLastNameEntry = new Entry();
            singleUserMobileEntry = new Entry();

            singleUserCreationGrid.Add(singleUserFirstNameLabel, 0, 0);
            singleUserCreationGrid.Add(singleUserLastNameLabel, 0, 1);
            singleUserCreationGrid.Add(singleUserMobileLabel, 0, 2);

            singleUserCreationGrid.Add(singleUserFirstNameEntry, 1, 0);
            singleUserCreationGrid.Add(singleUserLastNameEntry, 1, 1);
            singleUserCreationGrid.Add(singleUserMobileEntry, 1, 2);

            // Define the single user create button and span it over two columns
            Button singleUserCreateButton = new Button { Text = "Create User" };
            singleUserCreateButton.Clicked += SingleUserCreateButton_Clicked;

            singleUserCreationGrid.Add(singleUserCreateButton, 0, 5);
            singleUserCreationGrid.SetColumnSpan(singleUserCreateButton, 3);

            return singleUserCreationGrid;
        }


        private async void SingleUserCreateButton_Clicked(object sender, EventArgs e)
        {
            await HomePageEventHandlers.CreateSingleUser(
                    singleUserFirstNameEntry.Text,
                    singleUserLastNameEntry.Text,
                    singleUserMobileEntry.Text,
                    _activeDirectoryService,
                    _configService);
        }
    }
}
