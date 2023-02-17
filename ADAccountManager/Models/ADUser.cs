using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ADAccountManager.Utilities;
using System.DirectoryServices.ActiveDirectory;
using CsvHelper;
using System.Globalization;

namespace ADAccountManager.Models
{
    internal sealed class ADUser
    {
        private readonly PrincipalContext _context;

        // Constructor
        public ADUser(PrincipalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a user principal from the directory.
        /// </summary>
        /// <param name="userPrincipalName">Name of the user principal to be retrieved.</param>
        /// <returns>The user principal specified in the parameter.</returns>
        public UserPrincipal GetUser(string userPrincipalName)
        {
            UserPrincipal user = new UserPrincipal(_context);

            try
            {
                // Check argument for a null or empty value
                ArgumentNullException.ThrowIfNullOrEmpty(userPrincipalName, nameof(userPrincipalName));

                // Find the user using the name provided by the userPrincipalName parameter and returns it if found.
                user = UserPrincipal.FindByIdentity(_context, userPrincipalName);

                return user;
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return user;
            }
        }

        /// <summary>
        /// Check whether a user principal exists.
        /// </summary>
        /// <param name="userPrincipalName">User principal name of the user to be checked.</param>
        /// <returns>True if the user principal exists. False if the user principal does not exist</returns>
        public bool Exists(string userPrincipalName)
        {
            try
            {
                // Check argument for a null or empty value
                ArgumentNullException.ThrowIfNullOrEmpty(userPrincipalName);

                // Check whether a user principal exists. Return false if the user principal does exist
                using (var user = GetUser(userPrincipalName))
                {
                    if (user == null)
                        return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <param name="userPrincipalName">Principal name (such as name.surname) of the user to be deleted.</param>
        /// <returns>True if the deletion is successful. False if the deletion is unsuccessful.</returns>
        public bool DeleteUser(string userPrincipalName)
        {
            try
            {
                // Check argument for a null or empty value
                ArgumentNullException.ThrowIfNullOrEmpty(userPrincipalName);

                if (!Exists(userPrincipalName))
                {
                    Application.Current.MainPage.DisplayAlert("Error: User not found.",
                            $"The username [{userPrincipalName}] cannot be found in the directory.",
                            "OK");

                    return false;
                }

                // Delete a user
                using (UserPrincipal user = GetUser(userPrincipalName))
                {
                    user.Delete();
                }

                return true;
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
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
            string domain)
        {
            try
            {
                // Check arguments for null or empty values
                ArgumentNullException.ThrowIfNullOrEmpty(firstName);
                ArgumentNullException.ThrowIfNullOrEmpty(lastName);
                ArgumentNullException.ThrowIfNullOrEmpty(userPrincipalName);
                ArgumentNullException.ThrowIfNullOrEmpty(domain);

                // Check that parameters do not contain special characters (other than ., - and spaces)
                string[] parameters =
                {
                    firstName,
                    lastName,
                    userPrincipalName,
                    domain
                };

                if (!Utilities.ADPropertyValidator.ValidateParameters(parameters))
                    return false;

                // Check whether a user principal exists
                if (Exists(userPrincipalName))
                {
                    Application.Current.MainPage.DisplayAlert("Error: User already exists.",
                            $"The user [{userPrincipalName}] already exists in the directory.",
                            "OK");

                    return false;
                }

                // Create a new user
                using (UserPrincipal user = new UserPrincipal(_context))
                {
                    // Set user details and create the account
                    user.Name = userPrincipalName;
                    user.GivenName = firstName;
                    user.Surname = lastName;
                    user.UserPrincipalName = userPrincipalName + "@" + domain;
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
                Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }

        /// <summary>
        /// Create a new user account.
        /// </summary>
        /// <param name="firstName">Given name of the user.</param>
        /// <param name="lastName">Surname of the user.</param>
        /// <param name="userPrincipalName">User principal name, in the format "name.surname".</param>
        /// <param name="upn">Domain the user should be added to.</param>
        /// <returns>True if the user account creation is successful. False if the user account creation is unsuccessful.</returns>
        public bool CreateUsersFromCsv(string csvPath)
        {
            try
            {
                // Check arguments for null or empty values
                ArgumentNullException.ThrowIfNullOrEmpty(csvPath);

                // Read records from CSV and add users
                using (StreamReader reader = new StreamReader(csvPath))
                {
                    using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<User>();

                        foreach (var record in records)
                        {
                            using (UserPrincipal user = new UserPrincipal(_context))
                            {
                                user.Name = record.UserPrincipalName;
                                user.GivenName = record.FirstName;
                                user.Surname = record.LastName;
                                user.UserPrincipalName = record.UserPrincipalName + "@" + record.Domain;
                                user.SamAccountName = record.UserPrincipalName;
                                user.DisplayName = record.FirstName + " " + record.LastName;
                                user.Description = record.FirstName + " " + record.LastName;
                                user.Enabled = true;
                                user.Save();
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }
    }
}
