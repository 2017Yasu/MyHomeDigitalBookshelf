namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

public static class ObjectSchemaExtensions
{
    public static T? ToSchema<T>(this object obj, string prefix = "") where T : class, new()
    {
        if (obj is null || obj is not IDictionary<string, object> objDict)
        {
            return null!;
        }

        var schema = new T();
        var schemaType = typeof(T);
        var properties = schemaType.GetProperties();

        foreach (var kvp in objDict)
        {
            var columnName = kvp.Key.ToLowerInvariant();
            if (!string.IsNullOrEmpty(prefix) && columnName.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            {
                columnName = columnName[prefix.Length..];
            }
            var propName = columnName.Replace("_", "");
            var prop = properties.FirstOrDefault(p => p.Name.Equals(propName, StringComparison.InvariantCultureIgnoreCase));
            if (prop != null && kvp.Value != null && prop.CanWrite)
            {
                prop.SetValue(schema, Convert.ChangeType(kvp.Value, prop.PropertyType));
            }
        }

        return schema;
    }
}
