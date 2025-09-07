namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

/// <summary>
/// Value object representing a price with validation for non-negative values.
/// </summary>
public class Price
{
    /// <summary>
    /// Gets the price value as a decimal.
    /// </summary>
    public decimal Value { get; }

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
