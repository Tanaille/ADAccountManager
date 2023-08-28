using ADAccountManager.Models;
using ADAccountManager.Utilities.Services;
using ADAccountManager.Utilities.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADAccountManager.Views.HomePage
{
    internal static class HomePageEventHandlers
    {
        internal static async Task<bool> CreateSingleUser(string firstName, string lastName, string mobile, IActiveDirectoryService _context, IConfigService cService)
        {
            string userPrincipalName = firstName.ToLower() + "." + lastName.ToLower();
            userPrincipalName = Regex.Replace(userPrincipalName, @"\s+", ""); // Strip whitespace from the UPN.
            string mailDomain = cService.GetConfig().MailDomain;
            
            IUserPrincipalCreator userPrincipalCreator = new UserPrincipalCreator(_context);

            ADUser user = new ADUser()
            {
                FirstName = firstName,
                LastName = lastName,
                UserPrincipalName = userPrincipalName,
                Domain = mailDomain,
                MobilePhone = mobile,
            };

            try
            {
                var result = await userPrincipalCreator.CreateUserPrincipalAsync(user);

                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("Created users", "The following user principals have been created:\n\n" + user.FirstName + " " + user.LastName, "OK");
                    return true;
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert("User creation failed", "The following user principals could not be created:\n\n" + user.FirstName + " " + user.LastName, "OK");
                    return false;
                }
            }
            catch (Exception ex)
            {
                object userMessage = ex.Data["UserMessage"];
                await Application.Current.MainPage.DisplayAlert("Error: " + ex.Message, userMessage.ToString(), "OK");
                return false;
            }
        }
    }
}
