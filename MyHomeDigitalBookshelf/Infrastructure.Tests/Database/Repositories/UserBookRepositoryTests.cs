using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class UserBookRepositoryTests : RepositoryTestBase
{
    private readonly UserBookRepository _repository;
    private readonly BookRepository _bookRepository;
    private readonly UserRepository _userRepository;
    private readonly BookshelfRepository _bookshelfRepository;

    public UserBookRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repository = new UserBookRepository(CreateLogger<UserBookRepository>(), GetConnectionProvider());
        _bookRepository = new BookRepository(CreateLogger<BookRepository>(), GetConnectionProvider());
        _userRepository = new UserRepository(CreateLogger<UserRepository>(), GetConnectionProvider());
        _bookshelfRepository = new BookshelfRepository(CreateLogger<BookshelfRepository>(), GetConnectionProvider());
    }

    [Fact]
    public async Task AddAsync_Should_Add_UserBook()
    {
        // Arrange
        var (user, book) = await CreateTestEntities();
        var userBook = UserBook.CreateNew(
            user.Id,
            book.Id,
            ownership: true,
            readingStatus: ReadingStatus.Reading,
            loanStatus: null,
            purchaseDate: new DateTime(2025, 1, 1),
            price: new Domain.ValueObjects.Price(2500));

        // Act
        var result = await _repository.AddAsync(userBook);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userBook.UserId, result.UserId);
        Assert.Equal(userBook.BookId, result.BookId);
        Assert.Equal(userBook.Ownership, result.Ownership);
        Assert.Equal(userBook.ReadingStatus, result.ReadingStatus);
        Assert.Equal(userBook.LoanStatus, result.LoanStatus);
        Assert.Equal(userBook.PurchaseDate, result.PurchaseDate);
        Assert.Equal(userBook.Price, result.Price);
    }

    [Fact]
    public async Task GetAsync_Should_Return_UserBook()
    {
        // Arrange
        var (user, book) = await CreateTestEntities();
        var userBook = await CreateTestUserBookAsync(user.Id, book.Id);

        // Act
        var result = await _repository.GetAsync(userBook.UserId, userBook.BookId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userBook.UserId, result.UserId);
        Assert.Equal(userBook.BookId, result.BookId);
        Assert.Equal(userBook.Ownership, result.Ownership);
        Assert.Equal(userBook.ReadingStatus, result.ReadingStatus);
        Assert.Equal(userBook.LoanStatus, result.LoanStatus);
        Assert.Equal(userBook.PurchaseDate, result.PurchaseDate);
        Assert.Equal(userBook.Price, result.Price);
    }

    [Fact]
    public async Task GetByUserAsync_Should_Return_UserBooks()
    {
        // Arrange
        var (user, book1) = await CreateTestEntities();
        var (_, book2) = await CreateTestEntities();
        await CreateTestUserBookAsync(user.Id, book1.Id);
        await CreateTestUserBookAsync(user.Id, book2.Id);

        // Act
        var results = await _repository.GetByUserAsync(user.Id);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Length);
        Assert.All(results, r => Assert.Equal(user.Id, r.UserId));
    }

    [Fact]
    public async Task GetByBookAsync_Should_Return_UserBooks()
    {
        // Arrange
        var (user1, book) = await CreateTestEntities();
        var (user2, _) = await CreateTestEntities();
        await CreateTestUserBookAsync(user1.Id, book.Id);
        await CreateTestUserBookAsync(user2.Id, book.Id);

        // Act
        var results = await _repository.GetByBookAsync(book.Id);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Length);
        Assert.All(results, r => Assert.Equal(book.Id, r.BookId));
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_UserBook()
    {
        // Arrange
        var (user, book) = await CreateTestEntities();
        var userBook = await CreateTestUserBookAsync(user.Id, book.Id);
        var updatedUserBook = new UserBook(
            userBook.UserId,
            userBook.BookId,
            ownership: false,
            readingStatus: ReadingStatus.Completed,
            loanStatus: LoanStatus.Borrowed,
            purchaseDate: new DateTime(2025, 2, 1),
            price: new Domain.ValueObjects.Price(3000));

        // Act
        var result = await _repository.UpdateAsync(updatedUserBook);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedUserBook.UserId, result.UserId);
        Assert.Equal(updatedUserBook.BookId, result.BookId);
        Assert.Equal(updatedUserBook.Ownership, result.Ownership);
        Assert.Equal(updatedUserBook.ReadingStatus, result.ReadingStatus);
        Assert.Equal(updatedUserBook.LoanStatus, result.LoanStatus);
        Assert.Equal(updatedUserBook.PurchaseDate, result.PurchaseDate);
        Assert.Equal(updatedUserBook.Price, result.Price);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_UserBook()
    {
        // Arrange
        var (user, book) = await CreateTestEntities();
        var userBook = await CreateTestUserBookAsync(user.Id, book.Id);

        // Act
        await _repository.DeleteAsync(userBook.UserId, userBook.BookId);

        // Assert
        var result = await _repository.GetAsync(userBook.UserId, userBook.BookId);
        Assert.Null(result);
    }

    private async Task<(User user, Book book)> CreateTestEntities()
    {
        var user = User.CreateNew(
            $"test_user_{Guid.NewGuid():N}",
            new Domain.ValueObjects.Email($"test_{Guid.NewGuid():N}@example.com"),
            null,
            UserRole.Member);
        var createdUser = await _userRepository.AddAsync(user);

        var bookshelf = Bookshelf.CreateNew(
            $"test_bookshelf_{Guid.NewGuid():N}",
            "Test bookshelf");
        var createdBookshelf = await _bookshelfRepository.AddAsync(bookshelf);

        var book = Book.CreateNew(
            $"Test Book {Guid.NewGuid():N}",
            createdBookshelf.Id,
            ["Test Author"]);
        var createdBook = await _bookRepository.AddAsync(book);

        return (createdUser, createdBook);
    }

    private async Task<UserBook> CreateTestUserBookAsync(Guid userId, Guid bookId)
    {
        var userBook = UserBook.CreateNew(
            userId,
            bookId,
            ownership: true,
            readingStatus: ReadingStatus.Reading,
            loanStatus: null,
            purchaseDate: new DateTime(2025, 1, 1),
            price: new Domain.ValueObjects.Price(2500));

        return await _repository.AddAsync(userBook);
    }
}
