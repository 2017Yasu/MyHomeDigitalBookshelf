using System.Text.RegularExpressions;

namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

/// <summary>
/// Value object representing an ISBN (International Standard Book Number) with validation.
/// </summary>
public partial class Isbn : ValueObjectBase<string>, IEquatable<Isbn>
{
    /// <summary>
    /// Gets the ISBN value as a string.
    /// </summary>
    public override string Value { get; }

    public bool Equals(Isbn? other)
    {
        if (other is null) return false;
        string thisIsbn = Value.Replace("-", "").Trim();
        string otherIsbn = other.Value.Replace("-", "").Trim();
        return thisIsbn.Equals(otherIsbn);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is Isbn other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.Replace("-", "").Trim().GetHashCode();
    }

    public static bool operator ==(Isbn? left, Isbn? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Isbn? left, Isbn? right)
    {
        return !(left == right);
    }

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
