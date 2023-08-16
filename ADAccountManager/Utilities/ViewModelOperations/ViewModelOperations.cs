using ADAccountManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.ViewModelOperations
{
    internal static class ViewModelOperations
    {
        public static ObservableCollection<ADUser> PopulateObservableCollection(ADUser user)
        {
            ObservableCollection<ADUser> aDUsers = new ObservableCollection<ADUser>();
            aDUsers.Add(user);

            return aDUsers;
        }

        public static void SetViewBindings(CollectionView collectionView, ObservableCollection<ADUser> bindingSource)
        {
            collectionView.ItemsSource = bindingSource;
        }
    }
}
