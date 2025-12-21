# Repository Implementation Guide

## Overview

The repository implementation in MyHomeDigitalBookshelf follows a consistent pattern using Dapper for data access. Each repository consists of several components:

1. **Schema Classes** - Map database rows to C# objects
2. **SQL Classes** - Encapsulate SQL queries and Dapper execution
3. **Repository Implementation** - Coordinate data access and implement domain interfaces

## Components

### Schema Classes

Located in `Infrastructure/Database/Repositories/Schema/`
- Map database columns to C# properties
- Include ToEntity() method to convert to domain entity
- No business logic, pure data mapping

Example:
```csharp
public class SessionSchema
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    // ... other properties

    public Domain.Entities.Session ToEntity()
    {
        return new(
            id: Id,
            token: Token,
            // ... other properties
        );
    }
}
```

### SQL Classes

Located in `Infrastructure/Database/Repositories/Sql/`
- Inherit from QuerySqlBase (SELECT) or ExecSqlBase (INSERT/UPDATE/DELETE)
- Use Dapper for query execution
- Use parameterized queries
- Support transactions

Example:
```csharp
internal class GetSessionsSql : QuerySqlBase
{
    private const string Sql = @"
        SELECT id, token
        FROM sessions
        /**where**/";

    internal async Task<Schema.SessionSchema?> QuerySingleAsync(Guid id)
    {
        var builder = new SqlBuilder();
        builder = builder.Where("id = @id", new { id });
        // ... execution code
    }
}
```

### Repository Implementation

Located in `Infrastructure/Database/Repositories/`
- Inherit from RepositoryBase
- Use QueryAndTraceAsync/ExecuteAndTraceAsync for consistent logging
- Convert between schema and domain entities
- Handle transactions when needed

Example:
```csharp
public class SessionRepository : RepositoryBase, ISessionRepository
{
    public async Task<Session?> GetByIdAsync(Guid id)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetSessionsSql(conn).QuerySingleAsync(id);
                return schema?.ToEntity();
            },
            "Get Session By Id",
            $"id: {id}");
    }
}
```

## Testing

- Create test class inheriting from RepositoryTestBase
- Test all CRUD operations
- Verify entity mappings
- Test business rules and constraints
- Clean up test data automatically