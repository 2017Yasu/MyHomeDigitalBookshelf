using System.Text.RegularExpressions;

namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

/// <summary>
/// Value object representing an email address with format validation.
/// </summary>
public partial class Email : ValueObjectBase<string>
{
    /// <summary>
    /// Gets the email address value as a string.
    /// </summary>
    public override string Value { get; }

    public Email(string value)
    {
        if (!IsValidEmail(value))
        {
            throw new ArgumentException("Invalid email format.");
        }
        Value = value;
    }

    public static bool IsValidEmail(string value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && EmailPattern().IsMatch(value);
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailPattern();
}
