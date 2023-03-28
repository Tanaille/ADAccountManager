using ADAccountManager.Utilities.Validators;
using System.Text.RegularExpressions;

// Implementation of the IStringValidator interface for alphanumeric strings
public class AlphanumericStringValidator : IStringValidator
{
    // Define a static regular expression to match the allowed characters - alphanumeric, with , ' and - allowed.
    private static readonly Regex AllowedCharactersRegex = new Regex(@"^[a-zA-Z0-9,\'\-]+$");

    public ValidationResult IsValid(string input)
    {
        if (string.IsNullOrEmpty(input))
            return new ValidationResult(false, "Input string is null or empty.");

        bool isMatch = AllowedCharactersRegex.IsMatch(input);

        return isMatch
            ? new ValidationResult(true)
            : new ValidationResult(false, "Input string contains invalid characters.");
    }
}