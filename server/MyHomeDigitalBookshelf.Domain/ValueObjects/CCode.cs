using System.Text.RegularExpressions;

namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

public class CCode
{
    public string Value { get; }

    public CCode(string value)
    {
        if (!IsValidCCode(value))
            throw new ArgumentException("Invalid C-Code format.");
        Value = value;
    }

    public static bool IsValidCCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        return Regex.IsMatch(value, @"^C\d{4}$");
    }

    public override string ToString() => Value;
}
