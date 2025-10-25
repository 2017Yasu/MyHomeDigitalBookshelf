using Xunit.Abstractions;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class BookshelfUserRepositoryTests : RepositoryTestBase
{
    private readonly BookshelfUserRepository _repository;
    private readonly UserRepository _userRepository;
    private readonly BookshelfRepository _bookshelfRepository;

    public BookshelfUserRepositoryTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        _repository = new BookshelfUserRepository(CreateLogger<BookshelfUserRepository>(), GetConnectionProvider());
        _userRepository = new UserRepository(CreateLogger<UserRepository>(), GetConnectionProvider());
        _bookshelfRepository = new BookshelfRepository(CreateLogger<BookshelfRepository>(), GetConnectionProvider());
    }

    [Fact(DisplayName = "AddAsync should insert and return a bookshelf user")]
    public async Task AddAsync_ShouldInsertAndReturnBookshelfUser()
    {
        // Arrange
        var user = await CreateUserInDb();
        var bookshelf = await CreateBookshelfInDb();


        var bookshelfUser = BookshelfUser.CreateNew(user.Id, bookshelf.Id, BookshelfUserRole.Administrator);

        // Act
        var created = await _repository.AddAsync(bookshelfUser);

        // Assert
        Assert.NotNull(created);
        Assert.Equal(bookshelfUser.UserId, created.UserId);
        Assert.Equal(bookshelfUser.BookshelfId, created.BookshelfId);
        Assert.Equal(bookshelfUser.Role, created.Role);

        // Act - Get
        var fetched = await _repository.GetAsync(created.UserId, created.BookshelfId);

        // Assert
        Assert.NotNull(fetched);
        Assert.Equal(created.UserId, fetched.UserId);
        Assert.Equal(created.BookshelfId, fetched.BookshelfId);
        Assert.Equal(created.Role, fetched.Role);
    }

    [Fact(DisplayName = "GetByBookshelfAsync should return all users in a bookshelf")]
    public async Task GetByBookshelfAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var bookshelf = await CreateBookshelfInDb();
        var users = await Task.WhenAll(
            Enumerable.Range(0, 3).Select(_ => CreateUserInDb()));

        var bookshelfUsers = users
            .Select(u => BookshelfUser.CreateNew(u.Id, bookshelf.Id, BookshelfUserRole.Member))
            .ToArray();

        foreach (var bookshelfUser in bookshelfUsers)
        {
            await _repository.AddAsync(bookshelfUser);
        }

        // Act
        var result = await _repository.GetByBookshelfAsync(bookshelf.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookshelfUsers.Length, result.Length);
        foreach (var bookshelfUser in bookshelfUsers)
        {
            Assert.Contains(result, u => u.UserId == bookshelfUser.UserId && u.BookshelfId == bookshelfUser.BookshelfId);
        }
    }

    [Fact(DisplayName = "GetByUserAsync should return all bookshelves for a user")]
    public async Task GetByUserAsync_ShouldReturnAllBookshelves()
    {
        // Arrange
        var user = await CreateUserInDb();
        var bookshelves = await Task.WhenAll(
            Enumerable.Range(0, 3).Select(_ => CreateBookshelfInDb()));

        var bookshelfUsers = bookshelves
            .Select(b => BookshelfUser.CreateNew(user.Id, b.Id, BookshelfUserRole.Member))
            .ToArray();

        foreach (var bookshelfUser in bookshelfUsers)
        {
            await _repository.AddAsync(bookshelfUser);
        }

        // Act
        var result = await _repository.GetByUserAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookshelfUsers.Length, result.Length);
        foreach (var bookshelfUser in bookshelfUsers)
        {
            Assert.Contains(result, b => b.UserId == bookshelfUser.UserId && b.BookshelfId == bookshelfUser.BookshelfId);
        }
    }

    [Fact(DisplayName = "UpdateAsync should update role and return updated entity")]
    public async Task UpdateAsync_ShouldUpdateAndReturn()
    {
        // Arrange
        var user = await CreateUserInDb();
        var bookshelf = await CreateBookshelfInDb();
        var bookshelfUser = BookshelfUser.CreateNew(
            user.Id,
            bookshelf.Id,
            BookshelfUserRole.Member);
        await _repository.AddAsync(bookshelfUser);

        var updatedUser = new BookshelfUser(
            bookshelfUser.UserId,
            bookshelfUser.BookshelfId,
            BookshelfUserRole.Administrator,
            DateTime.UtcNow,
            DateTime.UtcNow);

        // Act
        var result = await _repository.UpdateAsync(updatedUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(BookshelfUserRole.Administrator, result.Role);

        // Verify in database
        var fetched = await _repository.GetAsync(result.UserId, result.BookshelfId);
        Assert.NotNull(fetched);
        Assert.Equal(BookshelfUserRole.Administrator, fetched.Role);
    }

    [Fact(DisplayName = "DeleteAsync should remove the bookshelf user")]
    public async Task DeleteAsync_ShouldRemoveBookshelfUser()
    {
        // Arrange
        var user = await CreateUserInDb();
        var bookshelf = await CreateBookshelfInDb();
        var bookshelfUser = BookshelfUser.CreateNew(
            user.Id,
            bookshelf.Id,
            BookshelfUserRole.Member);
        await _repository.AddAsync(bookshelfUser);

        // Act
        await _repository.DeleteAsync(bookshelfUser.UserId, bookshelfUser.BookshelfId);

        // Assert
        var result = await _repository.GetAsync(bookshelfUser.UserId, bookshelfUser.BookshelfId);
        Assert.Null(result);
    }

    private async Task<User> CreateUserInDb()
    {
        var user = User.CreateNew(
            username: "test_" + Guid.NewGuid().ToString("N"),
            email: new Email($"test_{Guid.NewGuid():N}@example.com"),
            passwordHash: null,
            role: UserRole.Member);

        var created = await _userRepository.AddAsync(user);
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
