# Coding Standards and Best Practices

## Clean Architecture Implementation

### Domain Layer
- Entities are immutable with private setters
- Use constructor validation for business rules
- Provide static factory methods for entity creation
- Use value objects for complex properties
- Implement proper ToString() methods using ClassUtilities
- Document public properties with XML comments
- Keep entities focused on core business logic

Example:
```csharp
public class Book
{
    public Guid Id { get; }
    public string Title { get; }
    public ValueObjects.Isbn? Isbn { get; }
    
    private Book(string title) 
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title must not be empty", nameof(title));
        Title = title;
    }

    public static Book Create(string title) => new(title);
}
```

### Infrastructure Layer

#### Repository Pattern Implementation
1. **Base Classes**
   - Use RepositoryBase for common functionality
   - Implement proper error handling and logging
   - Use QueryAndTraceAsync for read operations
   - Use ExecuteAndTraceAsync for write operations with transactions

2. **Repository Structure**
   - Schema classes for database mapping
   - SQL classes for query encapsulation
   - Repository implementations for domain interface
   
3. **Logging Pattern**
   - Log operation start with parameters
   - Log SQL errors with detailed information
   - Log operation completion
   - Use structured logging with proper context

4. **Error Handling**
   - Catch and log specific database exceptions
   - Proper transaction management
   - Consistent error propagation

Example Repository:
```csharp
public class BookRepository : RepositoryBase, IBookRepository
{
    public async Task<Book> AddAsync(Book book)
    {
        return await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new AddBookSql(conn, tran).ExecuteAsync(book);
                return schema.ToEntity();
            },
            "Add New Book",
            book.ToString());
    }
}
```

### Application Layer
- Use CQRS pattern with Commands and Queries
- Implement proper validation
- Handle business logic coordination
- Use dependency injection
- Implement proper service interfaces

## Coding Conventions

### Naming Conventions
- PascalCase for:
  - Public members
  - Class names
  - Interface names (prefix with 'I')
  - Property names
- camelCase for:
  - Private fields
  - Parameters
  - Local variables

### Documentation
- XML documentation for public APIs
- Clear and concise summaries
- Document parameters when not self-evident
- Include usage examples for complex APIs

### File Organization
- One class per file
- Group related files in feature folders
- Maintain consistent file structure
- Use proper namespacing

### Testing
- Write meaningful test names
- Test all CRUD operations
- Verify entity mappings
- Test business rules
- Clean up test data
- Use proper test categories

## TypeScript/React Conventions

### Component Structure
- Use functional components with hooks
- Implement proper TypeScript interfaces
- Follow React best practices
- Use proper state management

### File Organization
- Feature-based structure
- Shared components in common
- Clear separation of concerns
- Proper module organization

### Code Quality
- Use ESLint + Prettier
- Maintain consistent formatting
- Write meaningful comments
- Follow React patterns