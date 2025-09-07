namespace MyHomeDigitalBookshelf.Domain.ValueObjects;

public abstract class ValueObjectBase<TValue>
{
    abstract public TValue Value { get; }
}
