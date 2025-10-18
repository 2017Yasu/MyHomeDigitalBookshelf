# Development Commands

## Backend (.NET)

### Build
```bash
dotnet build MyHomeDigitalBookshelf/Api/MyHomeDigitalBookshelf.Api.csproj
```

### Run Tests
```bash
dotnet test MyHomeDigitalBookshelf/Infrastructure.Tests/MyHomeDigitalBookshelf.Infrastructure.Tests.csproj -v normal
```

### Database
```bash
# Start development database
docker compose -f compose.yml up db -d

# Start test database
docker compose -f compose.yml up db_test -d

# Clean up all containers and volumes
docker compose -f compose.yml down --volumes
```

## Frontend (React/TypeScript)

### Development
```bash
cd MyHomeDigitalBookshelf/Api/ClientApp
npm run dev          # Start development server
npm run build       # Build for production
npm run preview    # Preview production build
```

### Code Quality
```bash
cd MyHomeDigitalBookshelf/Api/ClientApp
npm run lint        # Check for linting issues
npm run lint:fix    # Fix linting issues
npm run check       # TypeScript type checking
npm run format      # Format code with Prettier
```