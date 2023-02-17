using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADAccountManager.Utilities
{
    static class ADPropertyValidator
    {
        /// <summary>
        /// Checks whether the provided strings meet the format requirments.
        /// These requirements are: alphanumeric, with no special characters except [, - '].
        /// </summary>
        /// <param name="strings">Array of strings to check for validity.</param>
        /// <returns>True if all strings are valid. False if any string is invalid, or the argument array is null or empty.</returns>
        public static bool ValidateParameters(string[] strings)
        {
            if ((strings == null) || (strings.Length == 0))
                return false;
            
            const string formatRegex = @"^[A-Za-z]+((\s)?((\'|\-|\.)?([A-Za-z])+))*$";

            try
            {
                foreach (string s in strings)
                {
                    if (!Regex.IsMatch(s, formatRegex))
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
    }
}
