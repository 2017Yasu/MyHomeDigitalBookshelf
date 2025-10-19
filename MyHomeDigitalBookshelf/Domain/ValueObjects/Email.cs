using System.Text.RegularExpressions;

namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

/// <summary>
/// Value object representing an email address with format validation.
/// </summary>
public partial class Email : ValueObjectBase<string>, IEquatable<Email>
{
    /// <summary>
    /// Gets the email address value as a string.
    /// </summary>
    public override string Value { get; }

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is Email other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.ToLowerInvariant().GetHashCode();
    }

    public static bool operator ==(Email? left, Email? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Email? left, Email? right)
    {
        return !(left == right);
    }

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
