# Infrastructure Layer Implementation Guide

## Overview

The Infrastructure layer in MyHomeDigitalBookshelf implements data access and external concerns using Clean Architecture principles. The implementation follows a structured pattern using Dapper for database access and comprehensive testing.

## Key Components

### 1. Database Settings

```csharp
public class DbSettings
{
    public string ConnectionString { get; }
    public string Host { get; }
    public string Database { get; }
    // ... other properties

    public DbSettings(
        string host,
        string database,
        string username,
        string password,
        int port = 5432,
        int connectionTimeoutSeconds = 15,
        int commandTimeoutSeconds = 30)
    {
        // Validate and initialize settings
    }
}
```

### 2. Repository Pattern Implementation

#### Base Repository
```csharp
public abstract class RepositoryBase
{
    protected ILogger Logger { get; }
    private readonly DbConnectionProvider _connectionProvider;

    protected async Task<T> QueryAndTraceAsync<T>(
        Func<DbConnection, Task<T>> queryFunc, 
        string operationDescription)
    {
        // Log operation start
        // Execute query with error handling
        // Log completion
    }

    protected async Task<T> ExecuteAndTraceAsync<T>(
        Func<DbConnection, DbTransaction, Task<T>> executeFunc, 
        string operationDescription)
    {
        // Log operation start
        // Execute in transaction with error handling
        // Log completion
    }
}
```

#### SQL Classes Structure

1. Base SQL Classes:
```csharp
internal abstract class QuerySqlBase
{
    protected readonly DbConnection _connection;
    protected readonly DbTransaction? _transaction;
}

internal abstract class ExecSqlBase : QuerySqlBase
{
    // Additional execution functionality
}
```

2. Query Implementation:
```csharp
internal class GetUserBooksSql : QuerySqlBase 
{
    private const string Sql = @"
        SELECT ub.user_id, ub.book_id, ...
        FROM user_books ub
        LEFT JOIN books b ON b.id = ub.book_id
        LEFT JOIN users u ON u.id = ub.user_id
        /**where**/";

    public async Task<Schema.UserBookSchema?> QuerySingleAsync(Guid userId, Guid bookId)
    {
        var builder = new SqlBuilder()
            .Where("ub.user_id = @userId AND ub.book_id = @bookId",
                new { userId, bookId });
        return (await QueryAsync(builder)).FirstOrDefault();
    }

    private async Task<Schema.UserBookSchema[]> QueryAsync(SqlBuilder builder)
    {
        var sql = builder.AddTemplate(Sql);
        return await _connection.QueryAsync<Schema.UserBookSchema>(
            sql.RawSql,
            sql.Parameters,
            _transaction);
    }
}
```

### 3. Schema Classes

1. Simple Entity Schema:
```csharp
public class BookSchema
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public Domain.Entities.Book ToEntity()
    {
        return new(Id, Title, /*...*/);
    }
}
```

2. Relationship Schema with Navigation:
```csharp
public class UserBookSchema
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public bool? Ownership { get; set; }
    public string? ReadingStatus { get; set; }
    
    // Navigation properties
    public UserSchema? User { get; set; }
    public BookSchema? Book { get; set; }

    public Domain.Entities.UserBook ToEntity()
    {
        return new(
            userId: UserId,
            bookId: BookId,
            ownership: Ownership,
            readingStatus: !string.IsNullOrEmpty(ReadingStatus) ?
                Enum.Parse<ReadingStatus>(ReadingStatus) : null,
            user: User?.ToEntity(),
            book: Book?.ToEntity());
    }
}
```

### 4. Testing Framework

#### Test Base Class
```csharp
public abstract class RepositoryTestBase : IAsyncDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly LoggerFactory _loggerFactory;

    protected ILogger<T> CreateLogger<T>()
    {
        return _loggerFactory.CreateLogger<T>();
    }

    protected DbConnectionProvider GetConnectionProvider()
    {
        // Initialize and return connection provider
    }

    public async ValueTask DisposeAsync()
    {
        // Clean up test data
    }
}
```

#### Repository Tests Pattern
```csharp
public class UserBookRepositoryTests : RepositoryTestBase
{
    private readonly UserBookRepository _repository;
    private readonly UserRepository _userRepository;
    private readonly BookRepository _bookRepository;

    [Fact]
    public async Task AddAsync_Should_Add_UserBook()
    {
        // Arrange: Create dependencies first
        var user = await CreateTestUserAsync();
        var book = await CreateTestBookAsync();
        var userBook = UserBook.CreateNew(user.Id, book.Id);

        // Act
        var result = await _repository.AddAsync(userBook);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userBook.UserId, result.UserId);
        // Verify all relevant properties
    }

    // Helper methods for test data setup
    private async Task<User> CreateTestUserAsync() { /*...*/ }
    private async Task<Book> CreateTestBookAsync() { /*...*/ }
}
```

## Implementation Guidelines

### 1. Repository Implementation

1. **Structure**
   - Inherit from RepositoryBase
   - Use dependency injection for logger and connection provider
   - Implement domain repository interface
   - Use schema classes for database mapping
   - Use SQL classes for query encapsulation

2. **Error Handling**
   - Use QueryAndTraceAsync for read operations
   - Use ExecuteAndTraceAsync for write operations
   - Log all operations with proper context
   - Handle SQL exceptions appropriately

3. **Database Operations**
   - Use parameterized queries
   - Implement proper transaction management
   - Use Dapper for efficient mapping
   - Use SqlBuilder for dynamic queries
   - Handle complex joins with proper schema mapping
   - Use enum string conversion for enum types

### 2. Schema Implementation

1. **Mapping Rules**
   - Match database column names exactly
   - Use nullable types appropriately
   - Implement ToEntity() for domain conversion
   - Handle enum conversions properly
   - Include navigation properties when needed
   - Keep schema classes internal to Infrastructure

2. **Value Objects**
   - Convert primitive types to domain value objects
   - Handle null values appropriately
   - Validate data during conversion
   - Keep conversion logic simple

### 3. SQL Implementation

1. **Query Organization**
   - One SQL class per operation type
   - Use SqlBuilder for dynamic queries
   - Keep SQL readable with proper formatting
   - Use proper joins for related data
   - Handle pagination if needed
   - Use proper column aliases for complex queries

2. **Performance Considerations**
   - Use appropriate indexes
   - Optimize join conditions
   - Handle large result sets properly
   - Use batch operations when possible
   - Consider query plan implications

### 4. Testing Guidelines

1. **Test Organization**
   - Group tests by operation
   - Test happy path and edge cases
   - Verify all entity properties
   - Test relationships and navigation
   - Clean up test data properly

2. **Test Data Management**
   - Create helper methods for test data
   - Handle dependencies properly
   - Use meaningful test values
   - Clean up in correct order
   - Handle cascading deletes

3. **Assertions**
   - Verify all relevant properties
   - Test null handling
   - Verify relationships
   - Test enum conversions
   - Verify value object conversions

## Example Usage

Complete implementation example for a relationship entity:

```csharp
// 1. Schema Class
public class UserBookSchema
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public bool? Ownership { get; set; }
    public string? ReadingStatus { get; set; }
    public UserSchema? User { get; set; }
    public BookSchema? Book { get; set; }

    public Domain.Entities.UserBook ToEntity() => new(/*...*/);
}

// 2. SQL Class
internal class GetUserBooksSql : QuerySqlBase
{
    private const string Sql = @"
        SELECT ub.*, b.*, u.*
        FROM user_books ub
        LEFT JOIN books b ON b.id = ub.book_id
        LEFT JOIN users u ON u.id = ub.user_id
        /**where**/";

    public async Task<UserBookSchema?> QuerySingleAsync(Guid userId, Guid bookId)
    {
        var builder = new SqlBuilder()
            .Where("ub.user_id = @userId AND ub.book_id = @bookId",
                new { userId, bookId });
        return (await QueryAsync(builder)).FirstOrDefault();
    }
}

// 3. Repository Implementation
public class UserBookRepository : RepositoryBase, IUserBookRepository
{
    public async Task<UserBook?> GetAsync(Guid userId, Guid bookId)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetUserBooksSql(conn)
                    .QuerySingleAsync(userId, bookId);
                return schema?.ToEntity();
            },
            "Get User Book",
            $"userId: {userId}, bookId: {bookId}");
    }
}

// 4. Repository Test
public class UserBookRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task GetAsync_Should_Return_UserBook_With_Navigation()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var book = await CreateTestBookAsync();
        var userBook = await CreateTestUserBookAsync(user.Id, book.Id);

        // Act
        var result = await _repository.GetAsync(user.Id, book.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.User);
        Assert.NotNull(result.Book);
        Assert.Equal(user.Id, result.User.Id);
        Assert.Equal(book.Id, result.Book.Id);
    }
}
```