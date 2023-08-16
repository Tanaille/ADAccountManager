using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials.UI;

namespace ADAccountManager.Utilities.FileOperations
{
    internal static class FileOperations
    {
        internal static async Task<FileResult> PickFile(PickOptions pickOptions)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(pickOptions);

                return result;
            }
            catch (FileNotFoundException e)
            {
                if (!e.Data.Contains("UserMessage"))
                    e.Data.Add("UserMessage", "An error occurred while attempting to read from the file. " +
                        "See the log file for more information.");

                throw;
            }
        }
    }
}
