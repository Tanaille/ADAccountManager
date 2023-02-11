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
        private string Username { get; set; }
        private readonly PrincipalContext context;
        private UserPrincipal user;

        public ADUser(PrincipalContext context)
        {
            this.context = context;
        }

        public bool SearchUser(string userPrincipalName)
        {
            user = UserPrincipal.FindByIdentity(context, userPrincipalName);

            if (user is not null)
                return true;

            else
                return false;
        }

        public bool DeleteUser(string userPrincipalName)
        {
            if (!SearchUser(userPrincipalName))
                return false;

            user.Delete();

            return true;
        }

        public bool CreateUser(
            string firstName, 
            string lastName, 
            string userPrincipalName, 
            string upn)
        {
            if (SearchUser(userPrincipalName))
                return false;

            using (UserPrincipal newUser = new UserPrincipal(context))
            {
                newUser.Name = userPrincipalName;
                newUser.Surname = lastName;
                newUser.UserPrincipalName = userPrincipalName + "@" + upn;
                newUser.SamAccountName = userPrincipalName;
                newUser.Description = firstName + " " + lastName;
                newUser.Enabled = true;
                newUser.Save();
            }

            return true;
        }
    }
}
