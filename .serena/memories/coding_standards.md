# Coding Standards and Best Practices

## General C# Conventions
- Target .NET 8 with nullable reference types and implicit usings enabled.
- Prefer one primary type per file and keep namespaces aligned with project/folder boundaries.
- Use PascalCase for public types and members; camelCase for parameters and local variables.
- Prefix interfaces with `I`.
- Keep public APIs documented when behavior is not obvious.
- Use small, focused services/repositories that match the existing feature folders.

## Architecture Boundaries
- `Domain` contains entities, enums, value objects, repository interfaces, and domain utilities.
- `Application` coordinates use cases through services and depends on domain abstractions.
- `Infrastructure` implements external concerns such as PostgreSQL repositories and external API/services.
- `Api` wires services, settings, controllers, Swagger, CORS, static files, and SPA fallback.
- `Utilities` contains shared attributes/extensions used across projects.
- Avoid moving business rules into API controllers or SQL classes.

## Domain Layer
- Keep entities focused on domain state and business invariants.
- Use value objects for structured domain concepts such as `Email`, `Isbn`, `Price`, and `CCode`.
- Validate constructor/factory inputs where invalid state would otherwise be possible.
- Use domain enums for constrained state such as roles, reading status, and loan status.
- Use `ClassUtilities`/existing helpers for consistent `ToString()` behavior when present.

## Application Layer
- Use feature folders such as `Books`, `Bookshelves`, `Categories`, `Sessions`, `UserBooks`, and `Users`.
- Keep application services responsible for orchestration and repository interaction.
- Register services through `ApplicationServiceExtensions`.
- Use dependency injection for service dependencies; avoid direct infrastructure construction.
- Application unit tests live under `Application.Tests` and use xUnit/Moq patterns already present in the repo.

## Infrastructure Layer
- Repository implementations live in `Infrastructure/Database/Repositories/` and implement domain repository interfaces.
- Use `RepositoryBase` plus `QueryAndTraceAsync` for reads and `ExecuteAndTraceAsync` for writes/transactions.
- Use Dapper and `Dapper.SqlBuilder` with parameterized SQL.
- Keep row mapping in `Schema` classes with `ToEntity()` conversions.
- Keep SQL execution details in operation-specific classes under `Repositories/Sql/`.
- Preserve the existing split between `QuerySqlBase` and `ExecSqlBase`.
- Use structured logging context through the existing repository base helpers.

## Database / Migrations
- PostgreSQL SQL files live under `sql/up` and `sql/down`.
- Migration filenames use four-digit numeric prefixes so `DbMigrator` can order and range-filter them.
- `compose.yml` provides `db` for development and `db_test` for integration tests.

## Testing
- Use xUnit for tests.
- Application service tests should mock repositories/dependencies with Moq.
- Infrastructure repository tests inherit from `RepositoryTestBase` and require the PostgreSQL test database.
- Test CRUD behavior, mappings, null handling, enum/value-object conversions, and relationship/navigation cases where relevant.
- Keep test names behavior-oriented, matching the existing `Method_Should_ExpectedBehavior` style.

## Frontend Note
No React/Vite/TypeScript client is currently checked in. Do not assume frontend lint/build/test commands exist unless a `package.json` or client project is added.