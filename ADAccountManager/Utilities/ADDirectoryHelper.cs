using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities
{
    internal sealed class ADDirectoryHelper
    {
        public async Task<List<string>> GetAllOUsInDomainAsync(string domainName)
        {
            List<string> ouList = new List<string>();
            string ldapPath = "LDAP://" + domainName;

            using (DirectoryEntry domain = new DirectoryEntry(ldapPath))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(domain))
                {
                    searcher.Filter = "(objectClass=organizationalUnit)";
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.PropertiesToLoad.Add("name");

                    var s = searcher.FindAll();

                    //var results = await Task.Run(() => searcher.FindAll());
                    //SearchResultCollection results = await Task.Run(() => searcher.FindAll());
                    //foreach (var result in results)
                    //{
                    //    //string ouName = result.Properties["name"][0].ToString();
                    //    //ouList.Add(ouName);

                    //    result.ToString();
                    //}
                }
            }

            return ouList;
        }
    }
}
