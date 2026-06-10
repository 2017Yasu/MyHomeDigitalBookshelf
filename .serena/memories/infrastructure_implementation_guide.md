# Infrastructure Layer Implementation Guide

## Overview
The Infrastructure project implements persistence and external services for the .NET 8 Clean Architecture solution. It depends on Domain, Application, and Utilities, and is registered from `InfrastructureServiceExtensions.AddInfrastructureServices`.

## Registration / Dependency Injection
- `Api/Program.cs` reads `Api.Settings.DbSettings` from the `Database` configuration section.
- `Api.Settings.DbSettings.ToInfrastructureDbSettings()` converts API configuration into `Infrastructure.Database.DbSettings`.
- `InfrastructureServiceExtensions` registers HTTP client support, the singleton database settings, repositories, and services via `Utilities.Extensions.AddAttributedServices`.
- Current infrastructure services include Google Books finder, SMTP email, password hashing, token service, `DbConnectionProvider`, and all repository implementations.

## Database Settings and Connections
- `Infrastructure/Database/DbSettings.cs` validates host, database, username, password, port, and timeout values.
- Defaults: port `5432`, connection timeout `15`, command timeout `30`.
- `ConnectionString` is generated with `NpgsqlConnectionStringBuilder` and pooling enabled.
- `DbConnectionProvider` is the repository entry point for opening PostgreSQL connections.

## Repository Pattern
Repositories live in `Infrastructure/Database/Repositories/` and implement interfaces from `Domain/Repositories/`.

Required structure:
- Repository class at `Repositories/<Entity>Repository.cs`
- Schema row mapper at `Repositories/Schema/<Entity>Schema.cs`
- SQL operation classes at `Repositories/Sql/`, split by Add/Get/Update/Delete operations where applicable
- Domain conversion through `Schema.ToEntity()`

Repository conventions:
- Inherit from `RepositoryBase`.
- Use `QueryAndTraceAsync` for read operations.
- Use `ExecuteAndTraceAsync` for write operations; it opens a transaction, commits on success, rolls back on failure, and converts duplicate SQL errors to `DuplicateEntityException` where implemented.
- Keep SQL parameterized and use Dapper/Dapper.SqlBuilder rather than string interpolation.
- Keep business logic out of SQL classes; SQL classes should only execute database operations and map rows.

## SQL Classes
- Query classes inherit from `QuerySqlBase`.
- Insert/update/delete classes inherit from `ExecSqlBase`.
- SQL files/classes should keep statements readable and use `SqlBuilder` for conditional `WHERE` clauses.
- Relationship queries should return schema objects with navigation schema properties when the repository needs domain navigation objects.

## Schema Classes
- Keep schema classes as database row mappers with public settable properties for Dapper.
- Match database columns using Dapper-compatible names/aliases.
- Convert primitive database values into domain value objects/enums in `ToEntity()`.
- Use nullable properties where database columns or joins can be null.
- Use `ObjectSchemaExtensions` where it already supports shared mapping behavior.

## External Services
- `Infrastructure/Api/GoogleBooksFinderService.cs` handles Google Books lookup concerns.
- `Infrastructure/Services/PasswordHasher.cs`, `SmtpEmailService.cs`, and `TokenService.cs` implement application-facing service contracts.
- Register new infrastructure services through `InfrastructureServiceExtensions` following the existing attributed-service pattern.

## Database Migrations
- SQL migration files live in `sql/up` and `sql/down`.
- Filenames start with a four-digit migration number so the migrator can sort and range-filter files.
- `MyHomeDigitalBookshelf.Tools.DbMigrator` executes SQL files against PostgreSQL.
- Use `--down true` with `sql/down` for rollback migrations, and `--from` / `--to` to restrict migration ranges.

## Infrastructure Tests
- Integration tests live under `Infrastructure.Tests/Database/Repositories/` and inherit from `RepositoryTestBase`.
- Tests require the `db_test` PostgreSQL service from `compose.yml` on host port `35432`.
- Repository tests should cover CRUD operations, domain/schema mapping, relationship loading, enum/value-object conversion, duplicate handling, and cleanup.