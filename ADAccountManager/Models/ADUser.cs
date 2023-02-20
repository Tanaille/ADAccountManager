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
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

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
                ArgumentException.ThrowIfNullOrEmpty(userPrincipalName, nameof(userPrincipalName));

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
                ArgumentException.ThrowIfNullOrEmpty(userPrincipalName);

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
        public async Task<bool> DeleteUserAsync(string userPrincipalName)
        {
            try
            {
                // Check argument for a null or empty value
                ArgumentException.ThrowIfNullOrEmpty(userPrincipalName);

                if (!Exists(userPrincipalName))
                {
                    await Application.Current.MainPage.DisplayAlert("Error: User not found.",
                            $"The username [{userPrincipalName}] cannot be found in the directory.",
                            "OK");

                    return false;
                }

                // Delete a user
                using (UserPrincipal user = GetUser(userPrincipalName))
                {
                    await Task.Run(() => user.Delete());
                }

                return true;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }

        /// <summary>
        /// Create a new user account.
        /// </summary>
        /// <param name="firstName">Given name of the user.</param>
        /// <param name="lastName">Surname of the user.</param>
        /// <param name="userPrincipalName">User principal name, in the format "name.surname".</param>
        /// <param name="domain">Domain the user should be added to.</param>
        /// <returns>True if the user account creation is successful. False if the user account creation is unsuccessful.</returns>
        public async Task<bool> CreateUserAsync(
            string firstName,
            string lastName,
            string userPrincipalName,
            string domain)
        {
            try
            {
                // Check arguments for null or empty values
                ArgumentException.ThrowIfNullOrEmpty(firstName);
                ArgumentException.ThrowIfNullOrEmpty(lastName);
                ArgumentException.ThrowIfNullOrEmpty(userPrincipalName);
                ArgumentException.ThrowIfNullOrEmpty(domain);

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
                    await Application.Current.MainPage.DisplayAlert("Error: User already exists.",
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
                    await Task.Run(() => user.Save());
                }

                return true;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }

        /// <summary>
        /// Create a new user account and add group memberships.
        /// </summary>
        /// <param name="firstName">Given name of the user.</param>
        /// <param name="lastName">Surname of the user.</param>
        /// <param name="userPrincipalName">User principal name, in the format "name.surname".</param>
        /// <param name="domain">Domain the user should be added to.</param>
        /// <param name ="groupPrincipalNames">List of groups the user should be added to.</param>
        /// <returns>True if the user account creation is successful. False if the user account creation is unsuccessful.</returns>
        public async Task<bool> CreateUserWithGroupsAsync(
            string firstName,
            string lastName,
            string userPrincipalName,
            string domain, 
            List<string> groupPrincipalNames)
        {
            try
            {
                if (groupPrincipalNames.Count == 0)
                    return false;

                bool userCreated = await CreateUserAsync(firstName, lastName, userPrincipalName, domain);

                if (!userCreated)
                    return false;

                UserPrincipal user = GetUser(userPrincipalName);

                // Add the user to the specified group
                foreach (string groupPrincipalName in groupPrincipalNames)
                {
                    ArgumentException.ThrowIfNullOrEmpty(groupPrincipalName);

                    ADGroup group = new ADGroup(new PrincipalContext(ContextType.Domain, "ferrum.local"));
                    await Task.Run(() => group.AddGroupMember(user, groupPrincipalName));
                }

                return true;
            }
            catch (Exception e)
            {
                // Delete the user if it has been created
                if (Exists(userPrincipalName))
                    await DeleteUserAsync(userPrincipalName);

                await Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }

        /// <summary>
        /// Create new user accounts from a CSV file.
        /// </summary>
        /// <param name="csvPath">The full file path of the CSV file.</param>
        /// <returns>True if the user account creation is successful. False if the user account creation is unsuccessful.</returns>
        public async Task<bool> CreateUsersFromCsvAsync(string csvPath)
        {
            try
            {
                // Check arguments for null or empty values, and whether the file exists
                ArgumentException.ThrowIfNullOrEmpty(csvPath);

                if (!File.Exists(csvPath))
                    throw new FileNotFoundException("File not found: " + csvPath);
                
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
                                await Task.Run(() => user.Save());

                                // Add the user to the specified group
                                ADGroup group = new ADGroup(new PrincipalContext(ContextType.Domain, "ferrum.local"));
                                await Task.Run(() => group.AddGroupMember(user, "StaffAccounts"));
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("An error has occurred", e.Message, "OK");
                return false;
            }
        }
    }
}
