using Moq;
using MyHomeDigitalBookshelf.Application.Books.Commands;
using MyHomeDigitalBookshelf.Application.Books.Queries;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using MyHomeDigitalBookshelf.Application.Books.Interfaces;

namespace MyHomeDigitalBookshelf.Application.Tests.Books;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _mockBookRepository;
    private readonly Mock<IUserBookRepository> _mockUserBookRepository;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<IBookFinderService> _bookFinderServiceMock;
    private readonly Application.Books.BookService _service;

    public BookServiceTests()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _mockUserBookRepository = new Mock<IUserBookRepository>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _bookFinderServiceMock = new Mock<IBookFinderService>();
        _service = new Application.Books.BookService(_mockBookRepository.Object, _mockUserBookRepository.Object, _mockCategoryRepository.Object, _bookFinderServiceMock.Object);
    }

    [Fact]
    public async Task AddBook_WithValidCommand_ReturnsCreatedBook()
    {
        // Arrange
        var command = new AddBookCommand(
            Title: "Test Book",
            BookshelfId: Guid.NewGuid(),
            Authors: ["Test Author"],
            Isbn: new Isbn("978-4-0000-0000-0"),
            Publisher: "Test Publisher",
            PublishDate: new DateTime(2023, 1, 1),
            Notes: "Test notes"
        );

        var expectedBook = Book.CreateNew(
            command.Title,
            command.BookshelfId,
            command.Authors,
            command.Isbn,
            command.Publisher,
            command.PublishDate,
            command.CCode,
            command.CategoryId,
            command.CoverImageUrl,
            command.Notes);

        _mockBookRepository.Setup(r => r.AddAsync(It.IsAny<Book>()))
            .ReturnsAsync((Book book) => new Book(Guid.NewGuid(), book.Title, book.BookshelfId, book.Authors, book.Isbn, book.Publisher, book.PublishDate, book.CCode, book.CategoryId, book.CoverImageUrl, book.Notes, DateTime.UtcNow, DateTime.UtcNow));

        // Act
        var result = await _service.AddBookAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Authors, result.Authors);
        Assert.Equal(command.Isbn, result.Isbn);
        Assert.Equal(command.Publisher, result.Publisher);
        Assert.Equal(command.PublishDate, result.PublishDate);
        Assert.Equal(command.Notes, result.Notes);

        _mockBookRepository.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact]
    public async Task AddBook_WithInvalidCategory_ThrowsArgumentException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new AddBookCommand(
            Title: "Test Book",
            BookshelfId: Guid.NewGuid(),
            CategoryId: categoryId
        );

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddBookAsync(command));
    }

    [Fact]
    public async Task SearchBooks_WithValidCriteria_ReturnsMatchingBooks()
    {
        // Arrange
        var query = new SearchBooksQuery(
            Title: "Test",
            Author: "Author",
            Isbn: "978-4-0000-0000-0"
        );

        var expectedBooks = new[]
        {
            Book.CreateNew(
                title: "Test Book 1",
                bookshelfId: Guid.NewGuid(),
                authors: ["Test Author"]),
            Book.CreateNew(
                title: "Test Book 2",
                bookshelfId: Guid.NewGuid(),
                authors: ["Test Author"])
        };

        _mockBookRepository.Setup(r => r.SearchAsync(
            It.Is<string?>(s => s == query.Title),
            It.Is<string?>(s => s == query.Author),
            It.Is<string?>(s => s == (query.Isbn != null ? query.Isbn.ToString() : null)),
            It.Is<Guid?>(g => g == query.CategoryId),
            It.Is<string?>(s => s == (query.CCode != null ? query.CCode.ToString() : null)),
            It.Is<Guid?>(g => g == query.OwnerId),
            It.IsAny<Guid?>(), // BookshelfId parameter, not used in this general search test
            It.Is<ReadingStatus?>(s => s == query.ReadingStatus)))
            .ReturnsAsync(expectedBooks);

        // Act
        var result = await _service.SearchBooksAsync(query);

        // Assert
        Assert.Equal(expectedBooks.Length, result.Length);
        _mockBookRepository.Verify(r => r.SearchAsync(
            It.Is<string?>(s => s == query.Title),
            It.Is<string?>(s => s == query.Author),
            It.Is<string?>(s => s == (query.Isbn != null ? query.Isbn.ToString() : null)),
            It.Is<Guid?>(g => g == query.CategoryId),
            It.Is<string?>(s => s == (query.CCode != null ? query.CCode.ToString() : null)),
            It.Is<Guid?>(g => g == query.OwnerId),
            It.IsAny<Guid?>(), // BookshelfId parameter, not used in this general search test
            It.Is<ReadingStatus?>(s => s == query.ReadingStatus)), Times.Once);    }

    [Fact]
    public async Task GetBookById_WithExistingBook_ReturnsBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var query = new GetBookByIdQuery(bookId);

        var expectedBook = Book.CreateNew(
            title: "Test Book",
            bookshelfId: Guid.NewGuid(),
            authors: ["Test Author"]);

        _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync(expectedBook);

        // Act
        var result = await _service.GetBookByIdAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBook.Title, result.Title);
        _mockBookRepository.Verify(r => r.GetByIdAsync(bookId), Times.Once);
    }

    [Fact]
    public async Task GetBookById_WithNonExistingBook_ReturnsNull()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var query = new GetBookByIdQuery(bookId);

        _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync((Book?)null);

        // Act
        var result = await _service.GetBookByIdAsync(query);

        // Assert
        Assert.Null(result);
        _mockBookRepository.Verify(r => r.GetByIdAsync(bookId), Times.Once);
    }

    [Fact]
    public async Task UpdateBook_WithValidCommand_ReturnsUpdatedBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var command = new UpdateBookCommand(
            Id: bookId,
            Title: "Updated Book",
            BookshelfId: Guid.NewGuid(),
            Authors: ["Updated Author"],
            Isbn: new Isbn("978-4-0000-0000-0"),
            Publisher: "Updated Publisher",
            PublishDate: new DateTime(2023, 1, 1),
            Notes: "Updated notes"
        );

        var existingBook = new Book(
            bookId,
            "Original Book",
            command.BookshelfId,
            ["Original Author"],
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(-1));

        var expectedBook = new Book(
            command.Id,
            command.Title,
            command.BookshelfId,
            command.Authors,
            command.Isbn,
            command.Publisher,
            command.PublishDate,
            command.CCode,
            command.CategoryId,
            command.CoverImageUrl,
            command.Notes,
            existingBook.CreatedAt,
            DateTime.UtcNow);

        _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync(existingBook);

        _mockBookRepository.Setup(r => r.UpdateAsync(It.IsAny<Book>()))
            .ReturnsAsync((Book book) => book);

        // Act
        var result = await _service.UpdateBookAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Authors, result.Authors);
        Assert.Equal(command.Isbn, result.Isbn);
        Assert.Equal(command.Publisher, result.Publisher);
        Assert.Equal(command.PublishDate, result.PublishDate);
        Assert.Equal(command.Notes, result.Notes);

        _mockBookRepository.Verify(r => r.GetByIdAsync(bookId), Times.Once);
        _mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact]
    public async Task UpdateBook_WithNonExistingBook_ReturnsNull()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var command = new UpdateBookCommand(
            Id: bookId,
            Title: "Updated Book",
            BookshelfId: Guid.NewGuid());

        _mockBookRepository.Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync((Book?)null);

        // Act
        var result = await _service.UpdateBookAsync(command);

        // Assert
        Assert.Null(result);
        _mockBookRepository.Verify(r => r.GetByIdAsync(bookId), Times.Once);
        _mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public async Task DeleteBook_WithValidCommand_CallsDeleteAsync()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var command = new DeleteBookCommand(bookId);

        _mockBookRepository.Setup(r => r.DeleteAsync(bookId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteBookAsync(command);

        // Assert
        _mockBookRepository.Verify(r => r.DeleteAsync(bookId), Times.Once);
    }

    [Fact]
    public async Task AddBookFromIsbnAsync_ShouldSucceed_WhenBookFoundByIsbn()
    {
        // Arrange
        var command = new AddBookFromIsbnCommand { Isbn = "978-0321765723", BookshelfId = Guid.NewGuid(), OwnerId = Guid.NewGuid() };
        var foundBook = Book.CreateNew("The Lord of the Rings", command.BookshelfId, new[] { "J.R.R. Tolkien" }, new Isbn("978-0321765723"));

        _bookFinderServiceMock.Setup(s => s.FindByIsbnAsync(command.Isbn)).ReturnsAsync(foundBook);
        _mockBookRepository.Setup(r => r.GetByIsbnAsync(It.IsAny<Isbn>())).ReturnsAsync((Book?)null);
                _mockBookRepository.Setup(r => r.AddAsync(It.IsAny<Book>()))
                    .ReturnsAsync((Book book) => new Book(Guid.NewGuid(), book.Title, book.BookshelfId, book.Authors, book.Isbn, book.Publisher, book.PublishDate, book.CCode, book.CategoryId, book.CoverImageUrl, book.Notes, DateTime.UtcNow, DateTime.UtcNow, book.Category));
        _mockUserBookRepository.Setup(r => r.AddAsync(It.IsAny<UserBook>())).ReturnsAsync((UserBook ub) => ub);

        // Act
        var result = await _service.AddBookFromIsbnAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(foundBook.Title, result.Title);
        _bookFinderServiceMock.Verify(s => s.FindByIsbnAsync(command.Isbn), Times.Once);
        _mockBookRepository.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
        _mockUserBookRepository.Verify(r => r.AddAsync(It.IsAny<UserBook>()), Times.Once);
    }

    [Fact]
    public async Task AddBookFromIsbnAsync_ShouldThrowException_WhenBookNotFoundByIsbn()
    {
        // Arrange
        var command = new AddBookFromIsbnCommand { Isbn = "978-0321765723", BookshelfId = Guid.NewGuid(), OwnerId = Guid.NewGuid() };

        _bookFinderServiceMock.Setup(s => s.FindByIsbnAsync(command.Isbn)).ReturnsAsync((Book?)null);
        _mockBookRepository.Setup(r => r.GetByIsbnAsync(It.IsAny<Isbn>())).ReturnsAsync((Book?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddBookFromIsbnAsync(command));
        _bookFinderServiceMock.Verify(s => s.FindByIsbnAsync(command.Isbn), Times.Once);
        _mockBookRepository.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Never);
        _mockUserBookRepository.Verify(r => r.AddAsync(It.IsAny<UserBook>()), Times.Never);
    }

    [Fact]
    public async Task GetBooksForBookshelfAsync_WithValidQuery_ReturnsFilteredAndPagedBooks()
    {
        // Arrange
        var bookshelfId = Guid.NewGuid();
        var query = new GetBooksForBookshelfQuery
        {
            BookshelfId = bookshelfId,
            Title = "Lord",
            Author = "Tolkien",
            PageNumber = 1,
            PageSize = 10,
            SortBy = "TitleAsc"
        };

        var expectedBooks = new[]
        {
            Book.CreateNew("The Lord of the Rings: Fellowship", bookshelfId, new[] { "J.R.R. Tolkien" }),
            Book.CreateNew("The Lord of the Rings: Two Towers", bookshelfId, new[] { "J.R.R. Tolkien" })
        };

        _mockBookRepository.Setup(r => r.SearchAsync(
                It.Is<string?>(s => s == query.Title),
                It.Is<string?>(s => s == query.Author),
                It.Is<string?>(s => s == query.Isbn), // Conversion to string for SearchAsync
                It.Is<Guid?>(g => g == query.CategoryId),
                It.IsAny<string?>(), // cCode, not in query
                It.Is<Guid?>(g => g == query.OwnerId),
                It.Is<Guid?>(g => g == query.BookshelfId),
                It.Is<ReadingStatus?>(s => s == query.ReadingStatus)
            ))
            .ReturnsAsync(expectedBooks);

        // Act
        var result = await _service.GetBooksForBookshelfAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBooks.Length, result.Length);
        _mockBookRepository.Verify(r => r.SearchAsync(
                It.Is<string?>(s => s == query.Title),
                It.Is<string?>(s => s == query.Author),
                It.Is<string?>(s => s == query.Isbn), // Conversion to string for SearchAsync
                It.Is<Guid?>(g => g == query.CategoryId),
                It.IsAny<string?>(), // cCode, not in query
                It.Is<Guid?>(g => g == query.OwnerId),
                It.Is<Guid?>(g => g == query.BookshelfId),
                It.Is<ReadingStatus?>(s => s == query.ReadingStatus)
            ), Times.Once);
    }
}
