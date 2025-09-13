using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Domain.Utilities;

public static class ClassUtilities
{
    public static string GetPropertiesInfo<T>(T obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        var properties = typeof(T).GetProperties();
        var propertyInfos = new System.Text.StringBuilder();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(obj);
            if (value == null)
            {
                propertyInfos.AppendLine($"{prop.Name}: null");
                continue;
            }

            if (ShouldDisplayType(prop.PropertyType))
            {
                propertyInfos.AppendLine($"{prop.Name}: {value}");
            }
            else
            {
                propertyInfos.AppendLine($"{prop.Name}: {{");
                propertyInfos.AppendLine(GetPropertiesInfo(value));
                propertyInfos.AppendLine("}");
            }
        }

        return propertyInfos.ToString();
    }

    private static bool ShouldDisplayType(Type type)
    {
        return type.IsPrimitive
            || type == typeof(string)
            || type.IsEnum
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(Guid)
            || IsValueObject(type);
    }

    private static bool IsValueObject(Type type)
    {
        return type.BaseType != null
            && type.BaseType.IsGenericType
            && type.BaseType.GetGenericTypeDefinition() == typeof(ValueObjectBase<>);
    }
}
