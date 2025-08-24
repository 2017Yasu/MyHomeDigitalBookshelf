using System.Text.RegularExpressions;

namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

public class Isbn
{
    public string Value { get; }

    public Isbn(string value)
    {
        if (!IsValidIsbn(value))
            throw new ArgumentException("Invalid ISBN format.");
        Value = value;
    }

    public static bool IsValidIsbn(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        var cleaned = value.Replace("-", "").Trim();
        return Regex.IsMatch(cleaned, @"^(\d{10}|\d{13})$");
    }

    public override string ToString() => Value;
}
