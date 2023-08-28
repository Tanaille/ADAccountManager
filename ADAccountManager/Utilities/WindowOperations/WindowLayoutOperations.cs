using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities.WindowLayoutOperations
{
    internal static class WindowLayoutOperations
    {
        internal static void ChangeWindowSize(int width, int height)
        {
            var appWindow = Application.Current.Windows.FirstOrDefault();

            if (appWindow != null)
            {
                appWindow.Width = width;
                appWindow.Height = height;
            }
        }
    }
}
