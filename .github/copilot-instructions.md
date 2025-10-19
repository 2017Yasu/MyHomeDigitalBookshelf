# GitHub Copilot Instructions

## Project Overview

My Home Digital Bookshelf is a web-based platform for personal and family book collection management built with .NET Core and React. The application follows Clean Architecture principles and uses Domain-Driven Design patterns.

## Development Best Practices

### Version Control

- Follow GitFlow branching strategy:
  - `main`: Production-ready code
  - `develop`: Integration branch
  - `feature/*`: New features
  - `bugfix/*`: Bug fixes
  - `release/*`: Release preparations
- Write meaningful commit messages following Conventional Commits
- Review code before merging
- Keep documentation up-to-date
- Use consistent code formatting

### .NET Core Best Practices

- Follow SOLID principles strictly
- Use async/await consistently:

  ```csharp
  // Good
  public async Task<Book> GetBookAsync(Guid id)
  {
      return await _repository.GetByIdAsync(id);
  }

  // Bad - blocking calls
  public Book GetBook(Guid id)
  {
      return _repository.GetById(id).Result;
  }
  ```

- Implement proper exception handling:
  ```csharp
  try
  {
      await _repository.SaveAsync(book);
  }
  catch (DbException ex)
  {
      _logger.LogError(ex, "Failed to save book {BookId}", book.Id);
      throw new BookshelfException("Failed to save book", ex);
  }
  ```
- Use dependency injection appropriately:

  ```csharp
  public class BookService
  {
      private readonly IBookRepository _repository;
      private readonly ILogger<BookService> _logger;

      public BookService(IBookRepository repository, ILogger<BookService> logger)
      {
          _repository = repository;
          _logger = logger;
      }
  }
  ```

- Configure proper logging levels
- Use middleware effectively
- Implement proper API versioning
- Use strongly-typed configuration

### React Best Practices

- Follow React Hooks rules:

  ```typescript
  // Good
  const BookDetail: React.FC<BookDetailProps> = ({ bookId }) => {
    const [book, setBook] = useState<Book | null>(null);

    useEffect(() => {
      const loadBook = async () => {
        const data = await fetchBook(bookId);
        setBook(data);
      };
      loadBook();
    }, [bookId]); // Proper dependency array
  };
  ```

- Implement proper error boundaries:

  ```typescript
  class ErrorBoundary extends React.Component<Props, State> {
    static getDerivedStateFromError(error: Error) {
      return { hasError: true, error };
    }

    render() {
      if (this.state.hasError) {
        return <ErrorDisplay error={this.state.error} />;
      }
      return this.props.children;
    }
  }
  ```

- Use proper state management:

  ```typescript
  // Local state
  const [filter, setFilter] = useState("");

  // Context for shared state
  const BookshelfContext = createContext<BookshelfContextType>(null);
  ```

- Implement proper TypeScript types:

  ```typescript
  interface Book {
    id: string;
    title: string;
    authors: string[];
    isbn?: string;
  }

  type BookshelfContextType = {
    books: Book[];
    addBook: (book: Book) => Promise<void>;
    removeBook: (id: string) => Promise<void>;
  };
  ```

## Key Technical Considerations

### Architecture Guidelines

- Follow Clean Architecture layers:
  - Domain: Core business logic and entities
  - Application: Use cases and interfaces
  - Infrastructure: External concerns and implementations
  - API: Web interface and frontend
- Use CQRS pattern for application layer operations
- Implement Repository pattern for data access
- Keep Domain layer free of external dependencies

### Backend Development (.NET)

- Use C# 12 features appropriately
- Follow standard C# naming conventions (PascalCase for public members)
- Add XML documentation for public APIs
- Implement proper validation and error handling
- Use strongly-typed configurations
- Write unit tests for business logic
- Write integration tests for infrastructure components

### Frontend Development (React)

- Use TypeScript with strict type checking
- Follow React Hooks best practices
- Implement proper error boundaries
- Use React Router for navigation
- Follow component composition patterns
- Style with Bootstrap 5 utilities
- Ensure responsive design
- Implement PWA features where appropriate

### Data Modeling

- Use proper C# record types for DTOs
- Implement value objects for domain concepts
- Use strongly-typed IDs
- Follow proper entity relationships
- Implement proper validation attributes
- Handle null values appropriately

### Database

- Use clean SQL in migrations
- Follow PostgreSQL best practices
- Implement proper indexing
- Use appropriate data types
- Handle migrations properly

### Testing

- Write meaningful test names
- Use appropriate test categories
- Mock external dependencies
- Use proper test data builders
- Follow Arrange-Act-Assert pattern
- Test edge cases and error conditions

## File Organization

```
MyHomeDigitalBookshelf/
├── Api/                 # Web API and frontend
│   ├── Controllers/     # API endpoints
│   └── ClientApp/      # React frontend
├── Application/        # Application services
├── Domain/            # Core business logic
│   ├── Entities/     # Domain entities
│   └── ValueObjects/ # Domain value objects
└── Infrastructure/   # External concerns
    └── Database/    # Data access
```

## Common Tasks

### Adding a New Entity

1. Create entity class in Domain/Entities
2. Add value objects if needed
3. Create repository interface in Domain
4. Implement repository in Infrastructure
5. Add database migration
6. Create DTOs in Application
7. Implement service layer in Application
8. Add API controller endpoints
9. Implement frontend components

### Adding API Endpoints

1. Create DTO models if needed
2. Implement controller action with proper HTTP verb
3. Add input validation
4. Implement error handling
5. Add XML documentation
6. Create integration tests
7. Update frontend API client

### Frontend Feature Development

1. Create new component(s)
2. Add TypeScript interfaces
3. Implement proper state management
4. Add error handling
5. Style with Bootstrap
6. Add loading states
7. Implement responsive design
8. Add proper testing

### Database Changes

1. Create new migration
2. Test migration up/down
3. Update entity configurations
4. Update repository implementation
5. Add integration tests
6. Test with sample data

## Examples

### Entity Definition

```csharp
public class Book
{
    public Guid Id { get; }
    public string Title { get; }
    public string[] Authors { get; }
    public ValueObjects.Isbn? Isbn { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }

    private Book(string title, string[] authors)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title must not be empty", nameof(title));

        Id = Guid.NewGuid();
        Title = title;
        Authors = authors ?? [];
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public static Book Create(string title, string[] authors)
        => new(title, authors);
}
```

### React Component

```typescript
interface BookListProps {
  books: Book[];
  onSelect: (book: Book) => void;
}

export const BookList: React.FC<BookListProps> = ({ books, onSelect }) => {
  if (!books.length) {
    return <div className="alert alert-info">No books found</div>;
  }

  return (
    <div className="row row-cols-1 row-cols-md-3 g-4">
      {books.map((book) => (
        <div key={book.id} className="col">
          <div className="card h-100">
            <div className="card-body">
              <h5 className="card-title">{book.title}</h5>
              <p className="card-text">{book.authors.join(", ")}</p>
              <button
                className="btn btn-primary"
                onClick={() => onSelect(book)}
              >
                View Details
              </button>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};
```
