# Coding Standards and Conventions

## C# Conventions

### Architecture
- Follow Clean Architecture principles
- Domain-driven design patterns
- CQRS pattern for application layer
- Repository pattern for data access

### Naming
- PascalCase for:
  - Class names
  - Public methods
  - Public properties
  - Public fields
- camelCase for:
  - Local variables
  - Parameters
  - Private fields
- Use meaningful descriptive names
- Prefix interfaces with 'I'

### Documentation
- XML documentation for public APIs
- Clear summary tags for public members
- Parameter documentation when not self-evident

### Code Organization
- One class per file
- Namespace matches folder structure
- Group related functionality in same namespace
- Keep classes focused and single-responsibility

## TypeScript/React Conventions

### File Structure
- Feature-based organization
- Components in separate files
- Shared components in common directory
- Pages/routes in dedicated directory

### Naming
- PascalCase for React components
- camelCase for:
  - Variables
  - Functions
  - Instances
- kebab-case for:
  - File names
  - Directory names

### Code Quality
- Use TypeScript types/interfaces
- ESLint + Prettier for formatting
- Import sorting
- React hooks rules enforcement