# My Home Digital Bookshelf

## Project Purpose
A web-based platform for individuals and families to manage personal book collections, track reading progress, and coordinate lending within shared bookshelf contexts.

## Current Tech Stack
- **Runtime:** .NET 8
- **Backend:** ASP.NET Core Web API with controllers and Swagger in development
- **Architecture:** Clean Architecture-style solution split across Api, Application, Domain, Infrastructure, Utilities, Tests, and Tools projects
- **Database:** PostgreSQL 15 via Docker Compose
- **Data Access:** Dapper, Dapper.SqlBuilder, Npgsql
- **Testing:** xUnit, Moq, coverlet collector
- **Migrations/SQL:** SQL files under `sql/up` and `sql/down`, executed by `MyHomeDigitalBookshelf.Tools.DbMigrator`
- **CI:** GitHub Actions workflow `.github/workflows/test-solution.yml` builds and tests the full solution against a PostgreSQL test service

## Solution Projects
- `MyHomeDigitalBookshelf/Api`: ASP.NET Core Web API; controllers for auth, books, bookshelves, and user books; serves static files from `wwwroot`
- `MyHomeDigitalBookshelf/Application`: application services for books, bookshelves, categories, sessions, user books, and users
- `MyHomeDigitalBookshelf/Domain`: entities, repository interfaces, value objects, and domain utilities
- `MyHomeDigitalBookshelf/Infrastructure`: database repositories, schemas, SQL classes, external API/services, and dependency injection registration
- `MyHomeDigitalBookshelf/Utilities`: shared attributes/extensions used by other projects
- `MyHomeDigitalBookshelf/Application.Tests`: application service unit tests
- `MyHomeDigitalBookshelf/Infrastructure.Tests`: repository integration tests against PostgreSQL
- `MyHomeDigitalBookshelf/Tools/DbMigrator`: command-line SQL migration runner

## Current Frontend State
The repository currently has `Api/wwwroot/index.html` for static serving but no checked-in React/Vite/TypeScript client (`package.json`, `vite.config.*`, and `tsconfig.json` are absent). Treat earlier references to React 19, Vite, and Bootstrap as planned or historical unless frontend files are reintroduced.

## Key Features / Domain Areas
- Book and metadata management, including ISBN and Japanese C-Code value objects
- Categories
- Users and identities
- Sessions
- Bookshelves and bookshelf membership/roles
- User-specific book ownership, reading status, and loan status

## Database Layout
- Docker Compose services: `db` on host port `5432`, `db_test` on host port `35432`
- SQL migration files are numbered with four-digit prefixes, e.g. `0000_init.sql`, `0001_create_initial_tables.sql`
- Up migrations live in `sql/up`; down migrations live in `sql/down`