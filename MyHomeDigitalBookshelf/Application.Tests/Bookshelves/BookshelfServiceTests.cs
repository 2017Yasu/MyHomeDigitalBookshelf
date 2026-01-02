using MyHomeDigitalBookshelf.Domain.ValueObjects;
using Moq;
using MyHomeDigitalBookshelf.Application.Bookshelves.Commands;
using MyHomeDigitalBookshelf.Application.Bookshelves.Queries;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Application.Common.Interfaces;

namespace MyHomeDigitalBookshelf.Application.Tests.Bookshelves;

public class BookshelfServiceTests
{
    private readonly Mock<IBookshelfRepository> _mockBookshelfRepository;
    private readonly Mock<IBookshelfUserRepository> _mockBookshelfUserRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Application.Bookshelves.BookshelfService _service;

    public BookshelfServiceTests()
    {
        _mockBookshelfRepository = new Mock<IBookshelfRepository>();
        _mockBookshelfUserRepository = new Mock<IBookshelfUserRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _service = new Application.Bookshelves.BookshelfService(
            _mockBookshelfRepository.Object,
            _mockBookshelfUserRepository.Object,
            _mockUserRepository.Object,
            _mockEmailService.Object); // Modified
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
        var query = new GetBookshelfByIdQuery { Id = bookshelfId };

        var expectedBookshelf = Bookshelf.CreateNew("Test Bookshelf", "Test description");

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
            Bookshelf.CreateNew("Bookshelf 1", "First bookshelf"),
            Bookshelf.CreateNew("Bookshelf 2", "Second bookshelf")
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
        var bookshelf = Bookshelf.CreateNew("Test Bookshelf", "Test description");
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
        var command = new UpdateBookshelfUserCommand(userId, bookshelfId, BookshelfUserRole.Administrator);

        var existingRelationship = new BookshelfUser(
            userId,
            bookshelfId,
            BookshelfUserRole.Member,
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(-1));

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

        var bookshelf = Bookshelf.CreateNew("Test Bookshelf", "Test description");
        var expectedUsers = new[]
        {
            BookshelfUser.CreateNew(Guid.NewGuid(), bookshelfId, BookshelfUserRole.Administrator),
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

    [Fact]
    public async Task InviteUserToBookshelfAsync_WithValidCommand_SendsEmailAndAddsUser()
    {
        // Arrange
        var bookshelfId = Guid.NewGuid();
        var invitingUserId = Guid.NewGuid();
        var invitedUserEmail = "invited@example.com";
        var command = new InviteUserToBookshelfCommand
        {
            BookshelfId = bookshelfId,
            InvitingUserId = invitingUserId,
            InvitedUserEmail = invitedUserEmail
        };

        var existingBookshelf = new Bookshelf(bookshelfId, "Test Bookshelf", "Test Description", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
        var invitingUser = new User(Guid.NewGuid(), "Inviter", new Email("inviter@example.com"), "hash", Domain.Entities.UserRole.Member, DateTime.UtcNow);
        var invitedUser = new User(Guid.NewGuid(), "Invited", new Email(invitedUserEmail), "hash", Domain.Entities.UserRole.Member, DateTime.UtcNow);

        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(bookshelfId)).ReturnsAsync(existingBookshelf);
        _mockUserRepository.Setup(r => r.GetByIdAsync(invitingUserId)).ReturnsAsync(invitingUser);
        _mockUserRepository.Setup(r => r.GetByEmailAsync(invitedUserEmail)).ReturnsAsync(invitedUser); // User already exists
        _mockBookshelfUserRepository.Setup(r => r.GetAsync(invitedUser.Id, bookshelfId)).ReturnsAsync((BookshelfUser?)null);
        _mockBookshelfUserRepository.Setup(r => r.AddAsync(It.IsAny<BookshelfUser>())).ReturnsAsync((BookshelfUser bu) => bu);
        _mockEmailService.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        await _service.InviteUserToBookshelfAsync(command);

        // Assert
        _mockBookshelfRepository.Verify(r => r.GetByIdAsync(bookshelfId), Times.Once);
        _mockUserRepository.Verify(r => r.GetByIdAsync(invitingUserId), Times.Once);
        _mockUserRepository.Verify(r => r.GetByEmailAsync(invitedUserEmail), Times.Once);
        _mockBookshelfUserRepository.Verify(r => r.GetAsync(invitedUser.Id, bookshelfId), Times.Once);
        _mockBookshelfUserRepository.Verify(r => r.AddAsync(It.Is<BookshelfUser>(bu => bu.UserId == invitedUser.Id && bu.BookshelfId == bookshelfId)), Times.Once);
        _mockEmailService.Verify(s => s.SendEmailAsync(invitedUserEmail, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task InviteUserToBookshelfAsync_ShouldThrowException_WhenBookshelfDoesNotExist()
    {
        // Arrange
        var command = new InviteUserToBookshelfCommand
        {
            BookshelfId = Guid.NewGuid(),
            InvitingUserId = Guid.NewGuid(),
            InvitedUserEmail = "invited@example.com"
        };

        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(command.BookshelfId)).ReturnsAsync((Bookshelf?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.InviteUserToBookshelfAsync(command));
        _mockBookshelfRepository.Verify(r => r.GetByIdAsync(command.BookshelfId), Times.Once);
        _mockEmailService.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _mockBookshelfUserRepository.Verify(r => r.AddAsync(It.IsAny<BookshelfUser>()), Times.Never);
    }
}
