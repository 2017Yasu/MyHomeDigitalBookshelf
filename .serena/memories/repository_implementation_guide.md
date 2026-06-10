# Repository Implementation Guide

## Current Repository Set
Repository interfaces live in `Domain/Repositories/`; implementations live in `Infrastructure/Database/Repositories/`.

Current implemented repositories:
- `BookRepository` / `IBookRepository`
- `BookshelfRepository` / `IBookshelfRepository`
- `BookshelfUserRepository` / `IBookshelfUserRepository`
- `CategoryRepository` / `ICategoryRepository`
- `SessionRepository` / `ISessionRepository`
- `UserBookRepository` / `IUserBookRepository`
- `UserIdentityRepository` / `IUserIdentityRepository`
- `UserRepository` / `IUserRepository`

## File Layout
For a new repository-backed entity, follow the existing three-part structure:
- `Infrastructure/Database/Repositories/<Entity>Repository.cs`
- `Infrastructure/Database/Repositories/Schema/<Entity>Schema.cs`
- `Infrastructure/Database/Repositories/Sql/<Operation><Entity>Sql.cs`

SQL support base classes:
- `QuerySqlBase` for SELECT-style operations
- `ExecSqlBase` for INSERT/UPDATE/DELETE-style operations

## Repository Class Pattern
- Implement the matching domain repository interface.
- Inherit from `RepositoryBase`.
- Accept `ILogger<TRepository>` and `DbConnectionProvider` through constructor injection, matching existing repositories.
- Use `QueryAndTraceAsync` for reads.
- Use `ExecuteAndTraceAsync` for writes so operations run in a transaction.
- Convert schemas to domain entities inside the repository, not in controllers or application services.

Example shape:
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

## Schema Classes
- Put schemas under `Infrastructure/Database/Repositories/Schema/`.
- Use Dapper-friendly public properties with setters.
- Keep schema classes free of business rules.
- Implement `ToEntity()` for domain conversion.
- Convert database strings/numbers into domain enums and value objects in `ToEntity()`.
- Use nullable properties for optional columns and left-joined navigation data.
- Follow existing relationship schema patterns for bookshelves/users/user books.

## SQL Classes
- Put SQL operation classes under `Infrastructure/Database/Repositories/Sql/`.
- Use parameterized SQL only.
- Use Dapper and `Dapper.SqlBuilder` for dynamic filtering.
- Keep methods narrowly named around the operation they perform, such as `QuerySingleAsync`, `QueryAsync`, or `ExecuteAsync`, matching nearby files.
- Keep SQL readable and explicit about selected columns/aliases, especially for joins.
- Pass transactions through write operations.

## Registration
New repositories must be registered in `Infrastructure/InfrastructureServiceExtensions.cs` alongside the existing repository implementations so DI can resolve them.

## Tests
- Add repository integration tests under `Infrastructure.Tests/Database/Repositories/`.
- Inherit from `RepositoryTestBase`.
- Use the `db_test` service and migrated schema.
- Cover create/read/update/delete paths, domain mapping, optional/null fields, enum and value-object conversion, relationship/navigation cases, and duplicate/constraint behavior when relevant.
- Clean up created data in dependency order, matching existing tests.