namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

public class Price
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Price cannot be negative.");
        Value = value;
    }

    public override string ToString() => Value.ToString("F2");
}
