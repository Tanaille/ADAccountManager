using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace ADAccountManager.Models
{
    internal class ADUser
    {
        private readonly PrincipalContext _context;
        private UserPrincipal user;

        // Constructor
        public ADUser(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Search for a user in the directory.
        /// </summary>
        /// <param name="userPrincipalName">Principal name (such as name.surname) of the user to be deleted.</param>
        /// <returns>True if the user account is found. False if the user account is not found.</returns>
        public bool SearchUser(string userPrincipalName)
        {
            // Check if a user account exists.
            user = UserPrincipal.FindByIdentity(_context, userPrincipalName);

            if (user is not null)
                return true;

            else
                return false;
        }

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <param name="userPrincipalName">Principal name (such as name.surname) of the user to be deleted.</param>
        /// <returns>True if the deletion is successful. False if the deletion is unsuccessful.</returns>
        public bool DeleteUser(string userPrincipalName)
        {
            // Return false if a user account does not exist
            if (!SearchUser(userPrincipalName))
                return false;

            user.Delete();

            return true;
        }

        /// <summary>
        /// Create a new user account.
        /// </summary>
        /// <param name="firstName">Given name of the user.</param>
        /// <param name="lastName">Surname of the user.</param>
        /// <param name="userPrincipalName">User principal name, in the format "name.surname".</param>
        /// <param name="upn">Domain the user should be added to.</param>
        /// <returns>True if the user account creation is successful. False if the user account creation is unsuccessful.</returns>
        public bool CreateUser(
            string firstName, 
            string lastName, 
            string userPrincipalName, 
            string upn)
        {
            try
            {
                // Check paramaters for nulls or empty strings
                ArgumentNullException.ThrowIfNullOrEmpty(firstName);
                ArgumentNullException.ThrowIfNullOrEmpty(lastName);
                ArgumentNullException.ThrowIfNullOrEmpty(userPrincipalName);
                ArgumentNullException.ThrowIfNullOrEmpty(upn);

                // Return false if a user account does not exist
                if (SearchUser(userPrincipalName))
                    return false;

                // Create a new user
                using (UserPrincipal user = new UserPrincipal(_context))
                {
                    // Set user details and create the account
                    user.Name = userPrincipalName;
                    user.GivenName = firstName;
                    user.Surname = lastName;
                    user.UserPrincipalName = userPrincipalName + "@" + upn;
                    user.SamAccountName = userPrincipalName;
                    user.DisplayName = firstName + " " + lastName;
                    user.Description = firstName + " " + lastName;
                    user.Enabled = true;
                    user.Save();
                }

                return true;
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("Argument exception", e.Message, "OK");
                return false;
            }

        }
    }
}
