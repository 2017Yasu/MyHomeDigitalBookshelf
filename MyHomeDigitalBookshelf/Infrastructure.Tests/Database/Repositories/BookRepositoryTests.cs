using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class BookRepositoryTests : RepositoryTestBase
{
    private readonly BookRepository _repository;
    private readonly BookshelfRepository _bookshelfRepository;

    public BookRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repository = new BookRepository(CreateLogger<BookRepository>(), GetConnectionProvider());
        _bookshelfRepository = new BookshelfRepository(CreateLogger<BookshelfRepository>(), GetConnectionProvider());
    }

    [Fact]
    public async Task AddAsync_Should_Add_Book()
    {
        // Arrange
        var bookshelf = await CreateBookshelfInDb();
        var book = Book.CreateNew(
            "Test Book",
            bookshelf.Id,
            ["Author 1", "Author 2"],
            new Isbn("9784873119656"),
            "Test Publisher",
            new DateTime(2025, 1, 1),
            new CCode("C3055"),
            null,
            "https://example.com/cover.jpg",
            "Test notes");

        // Act
        var result = await _repository.AddAsync(book);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(book.Title, result.Title);
        Assert.Equal(book.Authors, result.Authors);
        Assert.Equal(book.Isbn, result.Isbn);
        Assert.Equal(book.Publisher, result.Publisher);
        Assert.Equal(book.PublishDate, result.PublishDate);
        Assert.Equal(book.CCode, result.CCode);
        Assert.Equal(book.CategoryId, result.CategoryId);
        Assert.Equal(book.CoverImageUrl, result.CoverImageUrl);
        Assert.Equal(book.Notes, result.Notes);
        Assert.Equal(book.BookshelfId, result.BookshelfId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Book()
    {
        // Arrange
        var bookshelf = await CreateBookshelfInDb();
        var book = await CreateTestBookAsync(bookshelf.Id);

        // Act
        var result = await _repository.GetByIdAsync(book.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(book.Title, result.Title);
        Assert.Equal(book.Authors, result.Authors);
        Assert.Equal(book.Isbn, result.Isbn);
        Assert.Equal(book.Publisher, result.Publisher);
        Assert.Equal(book.PublishDate, result.PublishDate);
        Assert.Equal(book.CCode, result.CCode);
        Assert.Equal(book.CategoryId, result.CategoryId);
        Assert.Equal(book.CoverImageUrl, result.CoverImageUrl);
        Assert.Equal(book.Notes, result.Notes);
        Assert.Equal(book.BookshelfId, result.BookshelfId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_For_Nonexistent_Book()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Book()
    {
        // Arrange
        var bookshelf = await CreateBookshelfInDb();
        var book = await CreateTestBookAsync(bookshelf.Id);
        var updatedBook = new Book(
            book.Id,
            "Updated Title",
            book.BookshelfId,
            ["Updated Author"],
            new Isbn("9784873119649"),
            "Updated Publisher",
            new DateTime(2025, 2, 1),
            new CCode("C3056"),
            null,
            "https://example.com/updated-cover.jpg",
            "Updated notes",
            book.CreatedAt,
            DateTime.UtcNow);

        // Act
        var result = await _repository.UpdateAsync(updatedBook);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedBook.Title, result.Title);
        Assert.Equal(updatedBook.Authors, result.Authors);
        Assert.Equal(updatedBook.Isbn, result.Isbn);
        Assert.Equal(updatedBook.Publisher, result.Publisher);
        Assert.Equal(updatedBook.PublishDate, result.PublishDate);
        Assert.Equal(updatedBook.CCode, result.CCode);
        Assert.Equal(updatedBook.CategoryId, result.CategoryId);
        Assert.Equal(updatedBook.CoverImageUrl, result.CoverImageUrl);
        Assert.Equal(updatedBook.Notes, result.Notes);
        Assert.Equal(updatedBook.BookshelfId, result.BookshelfId);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_For_Nonexistent_Book()
    {
        // Arrange
        var bookshelf = await CreateBookshelfInDb();
        var book = Book.CreateNew(
            "Test Book",
            bookshelf.Id,
            ["Author"]);

        // Act
        var result = await _repository.UpdateAsync(book);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Book()
    {
        // Arrange
        var bookshelf = await CreateBookshelfInDb();
        var book = await CreateTestBookAsync(bookshelf.Id);

        // Act
        await _repository.DeleteAsync(book.Id);

        // Assert
        var result = await _repository.GetByIdAsync(book.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Books_By_Title()
    {
        // Arrange
        var num = DateTime.UtcNow.Ticks;
        var bookshelf = await CreateBookshelfInDb();
        var book1 = await CreateTestBookAsync(bookshelf.Id, title: $"Test Book {num} 1");
        var book2 = await CreateTestBookAsync(bookshelf.Id, title: $"Test Book {num} 2");
        await CreateTestBookAsync(bookshelf.Id, title: "Different Title");

        // Act
        var results = await _repository.SearchAsync($"Test Book {num}", null, null, null, null, null, null);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Length);
        Assert.Contains(results, b => b.Id == book1.Id);
        Assert.Contains(results, b => b.Id == book2.Id);
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Books_By_Author()
    {
        // Arrange
        var num = DateTime.UtcNow.Ticks;
        var bookshelf = await CreateBookshelfInDb();
        var book1 = await CreateTestBookAsync(bookshelf.Id, authors: [$"Test Author {num}", "Other Author"]);
        var book2 = await CreateTestBookAsync(bookshelf.Id, authors: [$"Test Author {num}"]);
        await CreateTestBookAsync(bookshelf.Id, authors: ["Different Author"]);

        // Act
        var results = await _repository.SearchAsync(null, $"Test Author {num}", null, null, null, null, null);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Length);
        Assert.Contains(results, b => b.Id == book1.Id);
        Assert.Contains(results, b => b.Id == book2.Id);
    }

    private async Task<Book> CreateTestBookAsync(
        Guid bookshelfId,
        string? title = null,
        string[]? authors = null)
    {
        var book = Book.CreateNew(
                title: title ?? "Test Book",
                bookshelfId,
                authors: authors ?? ["Test Author"],
                isbn: new Isbn("9784873119656"),
                publisher: "Test Publisher",
                publishDate: new DateTime(2025, 1, 1),
                cCode: new CCode("C3055"),
                coverImageUrl: "https://example.com/cover.jpg",
                notes: "Test notes");

        var created = await _repository.AddAsync(book);
        return created;
    }

    private async Task<Bookshelf> CreateBookshelfInDb()
    {
        var bookshelf = Bookshelf.CreateNew(
            name: "Test Bookshelf " + Guid.NewGuid().ToString("N"),
            description: "A bookshelf for testing purposes.");

        var created = await _bookshelfRepository.AddAsync(bookshelf);
        return created;
    }
}
