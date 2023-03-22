using System;
using System.Text.RegularExpressions;

// Define an interface for string validators
public interface IStringValidator
{
    // Method to check if the input string is valid
    ValidationResult IsValid(string input);
}

// Custom class to hold the validation result and an optional error message
public class ValidationResult
{
    public bool IsValid { get; }
    public string ErrorMessage { get; }

    public ValidationResult(bool isValid, string errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }
}

// Implementation of the IStringValidator interface for alphanumeric strings
public class AlphanumericStringValidator : IStringValidator
{
    // Define a static regular expression to match the allowed characters
    private static readonly Regex AllowedCharactersRegex = new Regex(@"^[a-zA-Z0-9,\'\-]+$");

    // Implement the IsValid method from the IStringValidator interface
    public ValidationResult IsValid(string input)
    {
        // Check if the input string is null or empty
        if (string.IsNullOrEmpty(input))
        {
            return new ValidationResult(false, "Input string is null or empty.");
        }

        // Check if the input string matches the allowed characters regex
        bool isMatch = AllowedCharactersRegex.IsMatch(input);

        return isMatch
            ? new ValidationResult(true)
            : new ValidationResult(false, "Input string contains invalid characters.");
    }
}