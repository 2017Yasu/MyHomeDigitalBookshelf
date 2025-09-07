using System.Text.RegularExpressions;

namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

/// <summary>
/// Value object representing an ISBN (International Standard Book Number) with validation.
/// </summary>
public partial class Isbn
{
    /// <summary>
    /// Gets the ISBN value as a string.
    /// </summary>
    public string Value { get; }

    public Isbn(string value)
    {
        if (!IsValidIsbn(value))
        {
            throw new ArgumentException("Invalid ISBN format.");
        }
        Value = value;
    }

    public static bool IsValidIsbn(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        var cleaned = value.Replace("-", "").Trim();
        return IsbnPattern().IsMatch(cleaned);
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^(\d{10}|\d{13})$")]
    private static partial Regex IsbnPattern();
}
