namespace MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class TransientServiceAttribute(string? key) : Attribute
{
    public string? Key { get; } = key;
}
