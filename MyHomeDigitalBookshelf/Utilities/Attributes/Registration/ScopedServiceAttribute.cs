namespace MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ScopedServiceAttribute(string? key) : Attribute
{
    public string? Key { get; } = key;
}
