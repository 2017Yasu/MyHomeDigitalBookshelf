using Moq;
using MyHomeDigitalBookshelf.Application.Bookshelves.Commands;
using MyHomeDigitalBookshelf.Application.Bookshelves.Queries;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Application.Tests.Bookshelves;

public class BookshelfServiceTests
{
    private readonly Mock<IBookshelfRepository> _mockBookshelfRepository;
    private readonly Mock<IBookshelfUserRepository> _mockBookshelfUserRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Bookshelves.BookshelfService _service;

    public BookshelfServiceTests()
    {
        _mockBookshelfRepository = new Mock<IBookshelfRepository>();
        _mockBookshelfUserRepository = new Mock<IBookshelfUserRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _service = new Bookshelves.BookshelfService(
            _mockBookshelfRepository.Object,
            _mockBookshelfUserRepository.Object,
            _mockUserRepository.Object);
    }

    [Fact]
    public async Task AddBookshelf_WithValidCommand_ReturnsCreatedBookshelf()
    {
        // Arrange
        var command = new AddBookshelfCommand(
            Name: "Test Bookshelf",
            Description: "Test description"
        );

        var expectedBookshelf = Bookshelf.CreateNew(
            command.Name,
            command.Description);

        _mockBookshelfRepository.Setup(r => r.AddAsync(It.IsAny<Bookshelf>()))
            .ReturnsAsync((Bookshelf bookshelf) => bookshelf);

        // Act
        var result = await _service.AddBookshelfAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Description, result.Description);

        _mockBookshelfRepository.Verify(r => r.AddAsync(It.IsAny<Bookshelf>()), Times.Once);
    }

    [Fact]
    public async Task GetBookshelfById_WithExistingBookshelf_ReturnsBookshelf()
    {
        // Arrange
        var bookshelfId = Guid.NewGuid();
        var query = new GetBookshelfByIdQuery(bookshelfId);

        var expectedBookshelf = Bookshelf.CreateNew("Test Bookshelf");

        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(bookshelfId))
            .ReturnsAsync(expectedBookshelf);

        // Act
        var result = await _service.GetBookshelfByIdAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBookshelf.Name, result.Name);
        _mockBookshelfRepository.Verify(r => r.GetByIdAsync(bookshelfId), Times.Once);
    }

    [Fact]
    public async Task GetAllBookshelves_ReturnsAllBookshelves()
    {
        // Arrange
        var query = new GetAllBookshelvesQuery();

        var expectedBookshelves = new[]
        {
            Bookshelf.CreateNew("Bookshelf 1"),
            Bookshelf.CreateNew("Bookshelf 2")
        };

        _mockBookshelfRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(expectedBookshelves);

        // Act
        var result = await _service.GetAllBookshelvesAsync(query);

        // Assert
        Assert.Equal(expectedBookshelves.Length, result.Length);
        _mockBookshelfRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task AddBookshelfUser_WithValidCommand_ReturnsCreatedRelationship()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookshelfId = Guid.NewGuid();
        var command = new AddBookshelfUserCommand(userId, bookshelfId, BookshelfUserRole.Member);

        var user = new User(userId, "testuser", null, null, UserRole.Member, DateTime.UtcNow);
        var bookshelf = Bookshelf.CreateNew("Test Bookshelf");
        var expectedBookshelfUser = BookshelfUser.CreateNew(userId, bookshelfId, command.Role);

        _mockUserRepository.Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(bookshelfId))
            .ReturnsAsync(bookshelf);
        _mockBookshelfUserRepository.Setup(r => r.GetAsync(userId, bookshelfId))
            .ReturnsAsync((BookshelfUser?)null);
        _mockBookshelfUserRepository.Setup(r => r.AddAsync(It.IsAny<BookshelfUser>()))
            .ReturnsAsync((BookshelfUser bu) => bu);

        // Act
        var result = await _service.AddBookshelfUserAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(bookshelfId, result.BookshelfId);
        Assert.Equal(command.Role, result.Role);

        _mockUserRepository.Verify(r => r.GetByIdAsync(userId), Times.Once);
        _mockBookshelfRepository.Verify(r => r.GetByIdAsync(bookshelfId), Times.Once);
        _mockBookshelfUserRepository.Verify(r => r.GetAsync(userId, bookshelfId), Times.Once);
        _mockBookshelfUserRepository.Verify(r => r.AddAsync(It.IsAny<BookshelfUser>()), Times.Once);
    }

    [Fact]
    public async Task AddBookshelfUser_WithExistingRelationship_ThrowsInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookshelfId = Guid.NewGuid();
        var command = new AddBookshelfUserCommand(userId, bookshelfId, BookshelfUserRole.Member);

        var existingRelationship = BookshelfUser.CreateNew(userId, bookshelfId, BookshelfUserRole.Member);

        _mockBookshelfUserRepository.Setup(r => r.GetAsync(userId, bookshelfId))
            .ReturnsAsync(existingRelationship);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddBookshelfUserAsync(command));
    }

    [Fact]
    public async Task UpdateBookshelfUser_WithValidCommand_ReturnsUpdatedRelationship()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookshelfId = Guid.NewGuid();
        var command = new UpdateBookshelfUserCommand(userId, bookshelfId, BookshelfUserRole.Admin);

        var existingRelationship = BookshelfUser.CreateNew(userId, bookshelfId, BookshelfUserRole.Member);

        _mockBookshelfUserRepository.Setup(r => r.GetAsync(userId, bookshelfId))
            .ReturnsAsync(existingRelationship);
        _mockBookshelfUserRepository.Setup(r => r.UpdateAsync(It.IsAny<BookshelfUser>()))
            .ReturnsAsync((BookshelfUser bu) => bu);

        // Act
        var result = await _service.UpdateBookshelfUserAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(bookshelfId, result.BookshelfId);
        Assert.Equal(command.NewRole, result.Role);

        _mockBookshelfUserRepository.Verify(r => r.GetAsync(userId, bookshelfId), Times.Once);
        _mockBookshelfUserRepository.Verify(r => r.UpdateAsync(It.IsAny<BookshelfUser>()), Times.Once);
    }

    [Fact]
    public async Task RemoveBookshelfUser_WithValidCommand_CallsDeleteAsync()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookshelfId = Guid.NewGuid();
        var command = new RemoveBookshelfUserCommand(userId, bookshelfId);

        var existingRelationship = BookshelfUser.CreateNew(userId, bookshelfId, BookshelfUserRole.Member);

        _mockBookshelfUserRepository.Setup(r => r.GetAsync(userId, bookshelfId))
            .ReturnsAsync(existingRelationship);
        _mockBookshelfUserRepository.Setup(r => r.DeleteAsync(userId, bookshelfId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.RemoveBookshelfUserAsync(command);

        // Assert
        _mockBookshelfUserRepository.Verify(r => r.GetAsync(userId, bookshelfId), Times.Once);
        _mockBookshelfUserRepository.Verify(r => r.DeleteAsync(userId, bookshelfId), Times.Once);
    }

    [Fact]
    public async Task RemoveBookshelfUser_WithNonExistingRelationship_ThrowsArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookshelfId = Guid.NewGuid();
        var command = new RemoveBookshelfUserCommand(userId, bookshelfId);

        _mockBookshelfUserRepository.Setup(r => r.GetAsync(userId, bookshelfId))
            .ReturnsAsync((BookshelfUser?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.RemoveBookshelfUserAsync(command));
    }

    [Fact]
    public async Task GetBookshelfUsers_WithValidQuery_ReturnsUsers()
    {
        // Arrange
        var bookshelfId = Guid.NewGuid();
        var query = new GetBookshelfUsersQuery(bookshelfId);

        var bookshelf = Bookshelf.CreateNew("Test Bookshelf");
        var expectedUsers = new[]
        {
            BookshelfUser.CreateNew(Guid.NewGuid(), bookshelfId, BookshelfUserRole.Admin),
            BookshelfUser.CreateNew(Guid.NewGuid(), bookshelfId, BookshelfUserRole.Member)
        };

        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(bookshelfId))
            .ReturnsAsync(bookshelf);
        _mockBookshelfUserRepository.Setup(r => r.GetByBookshelfAsync(bookshelfId))
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _service.GetBookshelfUsersAsync(query);

        // Assert
        Assert.Equal(expectedUsers.Length, result.Length);
        _mockBookshelfRepository.Verify(r => r.GetByIdAsync(bookshelfId), Times.Once);
        _mockBookshelfUserRepository.Verify(r => r.GetByBookshelfAsync(bookshelfId), Times.Once);
    }
}
