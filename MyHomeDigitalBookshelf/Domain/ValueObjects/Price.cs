namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

/// <summary>
/// Value object representing a price with validation for non-negative values.
/// </summary>
public class Price : ValueObjectBase<decimal>, IEquatable<Price>
{
    /// <summary>
    /// Gets the price value as a decimal.
    /// </summary>
    public override decimal Value { get; }

    public bool Equals(Price? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is Price other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Price? left, Price? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Price? left, Price? right)
    {
        return !(left == right);
    }

    public Price(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Price cannot be negative.");
        }
        Value = value;
    }

    public override string ToString() => Value.ToString("F2");
}
