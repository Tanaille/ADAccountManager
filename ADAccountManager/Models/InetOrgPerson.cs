using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchType = System.DirectoryServices.AccountManagement.MatchType;

namespace ADAccountManager.Models
{
    [DirectoryRdnPrefix("CN")]
    [DirectoryObjectClass("inetOrgPerson")]
    public class InetOrgPerson : UserPrincipal
    {
        // Inplement the constructor using the base class constructor. 
        public InetOrgPerson(PrincipalContext context) : base(context) 
        { 
            
        }

        // Implement the constructor with initialization parameters.    
        public InetOrgPerson(PrincipalContext context,
                             string samAccountName,
                             string password,
                             bool enabled)
                             : base(context,
                                    samAccountName,
                                    password,
                                    enabled)
        {

        }

        InetOrgPersonSearchFilter searchFilter;

        new public InetOrgPersonSearchFilter AdvancedSearchFilter
        {
            get
            {
                if (null == searchFilter)
                    searchFilter = new InetOrgPersonSearchFilter(this);

                return searchFilter;
            }
        }

        // Implement the overloaded search method FindByIdentity.
        public static new InetOrgPerson FindByIdentity(PrincipalContext context,
                                                       string identityValue)
        {
            return (InetOrgPerson)FindByIdentityWithType(context,
                                                         typeof(InetOrgPerson),
                                                         identityValue);
        }

        // Implement the overloaded search method FindByIdentity. 
        public static new InetOrgPerson FindByIdentity(PrincipalContext context,
                                                       IdentityType identityType,
                                                       string identityValue)
        {
            return (InetOrgPerson)FindByIdentityWithType(context,
                                                         typeof(InetOrgPerson),
                                                         identityType,
                                                         identityValue);
        }

        // Create the mobile phone property.    
        [DirectoryProperty("mobile")]
        public string MobilePhone
        {
            get
            {
                if (ExtensionGet("mobile").Length != 1)
                    return null;

                return (string)ExtensionGet("mobile")[0];
            }

            set
            {
                ExtensionSet("mobile", value);
            }
        }

        // Create the usage location property.    
        [DirectoryProperty("c")]
        public string UsageLocation
        {
            get
            {
                if (ExtensionGet("c").Length != 1)
                    return null;

                return (string)ExtensionGet("c")[0];
            }

            set
            {
                ExtensionSet("c", value);
            }
        }

        // Create the proxyAddresses property.
        [DirectoryProperty("proxyAddresses")]
        public string[] ProxyAddresses
        {
            get
            {
                object[] proxysRaw = ExtensionGet("proxyAddresses");
                string[] proxys = new string[proxysRaw.Length];

                foreach (object proxy in proxysRaw)
                    proxys.Append(proxy.ToString());

                return proxys;
            }

            set
            {
                ExtensionSet("proxyAddresses", value);
            }
        }

        // Create the logoncount property.    
        [DirectoryProperty("LogonCount")]
        public Nullable<int> LogonCount
        {
            get
            {
                if (ExtensionGet("LogonCount").Length != 1)
                    return null;

                return ((Nullable<int>)ExtensionGet("LogonCount")[0]);
            }
        }

    }

    public class InetOrgPersonSearchFilter : AdvancedFilters
    {
        public InetOrgPersonSearchFilter(Principal p) : base(p) { }
        public void LogonCount(int value, MatchType mt)
        {
            this.AdvancedFilterSet("LogonCount", value, typeof(int), mt);
        }
    }
}
