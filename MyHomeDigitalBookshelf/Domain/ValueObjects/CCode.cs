using System.Text.RegularExpressions;

namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

/// <summary>
/// Value object representing a Japanese C-Code classification with validation.
/// </summary>
public partial class CCode : ValueObjectBase<string>
{
    /// <summary>
    /// Gets the C-Code value as a string.
    /// </summary>
    public override string Value { get; }

    public CCode(string value)
    {
        if (!IsValidCCode(value))
        {
            throw new ArgumentException("Invalid C-Code format.");
        }
        Value = value;
    }

    public static bool IsValidCCode(string value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && CCodePattern().IsMatch(value);
    }

    /// <summary>
    /// Gets the audience category from the C-Code.
    /// </summary>
    /// <returns>The audience code from the C-Code</returns>
    public int GetAudience()
    {
        // C-Code format is 'C' followed by 4 digits
        // The first digit (0-9) represents the audience category
        // Extract and parse the first digit of the four-digit code
        if (int.TryParse(Value[0].ToString(), out int audience))
        {
            return audience;
        }
        return 99; // Return a default value for invalid formats
    }

    /// <summary>
    /// Gets the format category from the C-Code.
    /// </summary>
    /// <returns>The format category from the C-Code</returns>
    public int GetFormat()
    {
        // C-Code format is 'C' followed by 4 digits
        // The second digit (0-9) represents the audience category
        // Extract and parse the second digit of the four-digit code
        if (int.TryParse(Value[1].ToString(), out int format))
        {
            return format;
        }
        return 99; // Return a default value for invalid formats
    }

    /// <summary>
    /// Gets the genre category from the C-Code.
    /// </summary>
    /// <returns>The genre code from the C-Code.</returns>
    public int GetGenre()
    {
        // C-Code format is 'C' followed by 4 digits
        // Digits 3-4 (00-98) represent the genre category
        // Extract and parse the last two digits of the four-digit code
        if (int.TryParse(Value[2..], out int genre))
        {
            return genre;
        }
        return 99; // Return a default value for invalid formats
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^C\d{4}$", RegexOptions.IgnoreCase)]
    private static partial Regex CCodePattern();
}
