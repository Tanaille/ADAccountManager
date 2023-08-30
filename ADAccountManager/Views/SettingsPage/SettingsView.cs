using ADAccountManager.Models;
using ADAccountManager.Utilities.Services;
using ADAccountManager.Views.UserCreationView;
using System.Text.Json;

namespace ADAccountManager.Views.SettingsPage
{
    internal class SettingsView
    {
        private readonly IConfigService _test;
        public Entry DomainNameEntry { get; set; }
        public Entry MailDomainEntry { get; set; }
        public Entry DomainUserEntry { get; set; }
        public Entry DomainPasswordEntry { get; set; }
        public Entry DefaultDomainOuEntry { get; set; }

        private readonly StackLayout _userCreationStackLayout;

        public SettingsView(StackLayout userCreationStackLayout, IConfigService test)
        {
            _userCreationStackLayout = userCreationStackLayout;
            _test = test;
        }

        public Grid AddElements(IConfigService configService)
        {

            _test.GetConfig();
            // Store config info
            Config config = configService.GetConfig(); 

            // Define the settings GridLayout
            Grid settingsGridLayout = new Grid
            {
                Margin = 20,
                ColumnSpacing = 10,
                RowSpacing = 5,

                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Auto }, // Label column
                    new ColumnDefinition {Width = GridLength.Star }  // Entry column
                },

                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto},
                }
            };

            //// Define setting entries
            DomainNameEntry = new Entry();
            MailDomainEntry = new Entry();
            DomainUserEntry = new Entry();
            DomainPasswordEntry = new Entry { IsPassword = true };
            DefaultDomainOuEntry = new Entry();

            // Save settings button
            Button saveSettingsButton = new Button
            {
                Text = "Save",
                Margin = 10,
            };

            saveSettingsButton.Clicked += SaveSettingsButton_Clicked;
            
            // Populate grid cells with labels and entries
            settingsGridLayout.Add(new Label() { Text = "Domain name:", VerticalTextAlignment = TextAlignment.Center }, 0, 0);
            settingsGridLayout.Add(DomainNameEntry, 1, 0);

            settingsGridLayout.Add(new Label() { Text = "Mail domain:", VerticalTextAlignment = TextAlignment.Center }, 0, 1);
            settingsGridLayout.Add(MailDomainEntry, 1, 1);

            settingsGridLayout.Add(new Label() { Text = "Domain user:", VerticalTextAlignment = TextAlignment.Center }, 0, 2);
            settingsGridLayout.Add(DomainUserEntry, 1, 2);

            settingsGridLayout.Add(new Label() { Text = "Domain password:", VerticalTextAlignment = TextAlignment.Center }, 0, 3);
            settingsGridLayout.Add(DomainPasswordEntry, 1, 3);

            settingsGridLayout.Add(new Label() { Text = "Default OU:", VerticalTextAlignment = TextAlignment.Center }, 0, 4);
            settingsGridLayout.Add(DefaultDomainOuEntry, 1, 4);

            // Add "Save" button to the last grid row and span it over two columns
            settingsGridLayout.Add(saveSettingsButton, 0, 5);
            settingsGridLayout.SetColumnSpan(saveSettingsButton, 2);

            PopulateSettings(config);

            return settingsGridLayout;
        }

        private void SaveSettingsButton_Clicked(object sender, EventArgs e)
        {
            ConfigService configService = new ConfigService();

            Config config = new Config
            {
                DomainName = DomainNameEntry.Text,
                MailDomain = MailDomainEntry.Text,
                DomainUser = DomainUserEntry.Text,
                DomainPassword = DomainPasswordEntry.Text,
                DefaultDomainOU = DefaultDomainOuEntry.Text,
            };

            configService.SetConfig(config);

            _userCreationStackLayout.Clear();
            SingleUserCreationView singleUserCreationView = new SingleUserCreationView();
            _userCreationStackLayout.Add(singleUserCreationView.AddElements());
        }

        private void PopulateSettings(Config config)
        {
            DomainNameEntry.Text = config.DomainName;
            MailDomainEntry.Text = config.MailDomain;
            DomainUserEntry.Text = config.DomainUser;
            DomainPasswordEntry.Text = config.DomainPassword;
            DefaultDomainOuEntry.Text = config.DefaultDomainOU;
        }
    }
}
