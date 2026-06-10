# Development Commands

Run commands from the repository root unless noted otherwise.

## Restore / Build
```bash
dotnet restore
dotnet build MyHomeDigitalBookshelf.sln
```

Release build, matching CI:
```bash
dotnet build --no-restore --configuration Release
```

## Tests
Run all tests:
```bash
dotnet test MyHomeDigitalBookshelf.sln -v normal
```

Run application unit tests only:
```bash
dotnet test MyHomeDigitalBookshelf/Application.Tests/MyHomeDigitalBookshelf.Application.Tests.csproj -v normal
```

Run infrastructure integration tests only:
```bash
dotnet test MyHomeDigitalBookshelf/Infrastructure.Tests/MyHomeDigitalBookshelf.Infrastructure.Tests.csproj -v normal
```

CI-style test after a Release build:
```bash
dotnet test --verbosity normal --no-build --configuration Release
```

## Database
Start development database:
```bash
docker compose -f compose.yml up db -d
```

Start test database:
```bash
docker compose -f compose.yml up db_test -d
```

Stop and remove database containers and volumes:
```bash
docker compose -f compose.yml down --volumes
```

## Database Migrations
Apply up migrations to the local development database:
```bash
dotnet run --project ./MyHomeDigitalBookshelf/Tools/DbMigrator/ -- --host localhost --port 5432 --database my_home_bookshelves --user bookshelves_user --password mhdb_test --dir sql/up
```

Apply up migrations to the local test database:
```bash
dotnet run --project ./MyHomeDigitalBookshelf/Tools/DbMigrator/ -- --host localhost --port 35432 --database my_home_bookshelves --user test_user --password test --dir sql/up
```

Run down migrations by adding `--down true` and using `--dir sql/down`. Use `--from <number>` and `--to <number>` to limit migration number ranges.

## API
Run the ASP.NET Core API:
```bash
dotnet run --project MyHomeDigitalBookshelf/Api/MyHomeDigitalBookshelf.Api.csproj
```

Swagger is enabled in Development. The API CORS policy allows `http://localhost:8081` in Development.

## Frontend
There is currently no checked-in React/Vite client app or `package.json`. Do not use the old `Api/ClientApp` npm commands unless frontend files are reintroduced.