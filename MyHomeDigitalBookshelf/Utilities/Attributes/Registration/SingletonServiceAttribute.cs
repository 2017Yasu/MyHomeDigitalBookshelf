namespace MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class SingletonServiceAttribute(string? key = null) : Attribute
{
    public string? Key { get; } = key;
}
