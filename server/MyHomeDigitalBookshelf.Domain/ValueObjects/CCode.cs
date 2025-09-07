using System.Text.RegularExpressions;

namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

/// <summary>
/// Value object representing a Japanese C-Code classification with validation.
/// </summary>
public partial class CCode
{
    /// <summary>
    /// Gets the C-Code value as a string.
    /// </summary>
    public string Value { get; }

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

    public override string ToString() => Value;

    [GeneratedRegex(@"^C\d{4}$", RegexOptions.IgnoreCase)]
    private static partial Regex CCodePattern();
}
