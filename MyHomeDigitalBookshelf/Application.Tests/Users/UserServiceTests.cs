using Moq;
using MyHomeDigitalBookshelf.Application.Common.Interfaces;
using MyHomeDigitalBookshelf.Application.Users;
using MyHomeDigitalBookshelf.Application.Users.Commands;
using MyHomeDigitalBookshelf.Application.Users.Queries;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using System.Threading.Tasks;
using Xunit;

namespace MyHomeDigitalBookshelf.Application.Tests.Users;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserIdentityRepository> _userIdentityRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userIdentityRepositoryMock = new Mock<IUserIdentityRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _userService = new UserService(_userRepositoryMock.Object, _userIdentityRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldSucceed_WhenDataIsValid()
    {
        // Arrange
        var command = new CreateUserCommand { Username = "testuser", Email = "test@example.com", Password = "password123" };
        var hashedPassword = "hashed_password";

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User user) => new User(Guid.NewGuid(), user.Username, user.Email, user.PasswordHash, user.Role, DateTime.UtcNow));
        _passwordHasherMock.Setup(p => p.Hash(command.Password)).Returns(hashedPassword);

        // Act
        var result = await _userService.CreateUserAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Username, result.Username);
        _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u => u.PasswordHash == hashedPassword)), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrowException_WhenEmailIsTaken()
    {
        // Arrange
        var command = new CreateUserCommand { Username = "testuser", Email = "test@example.com", Password = "password123" };
        var existingUser = User.CreateNew("existinguser", new Email(command.Email), "hash", UserRole.Member);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateUserAsync(command));
    }

    [Fact]
    public async Task AuthenticateUserAsync_ShouldSucceed_WhenCredentialsAreValid()
    {
        // Arrange
        var query = new AuthenticateUserQuery { Email = "test@example.com", Password = "password123" };
        var hashedPassword = "hashed_password";
        var user = User.CreateNew("testuser", new Email(query.Email), hashedPassword, UserRole.Member);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email)).ReturnsAsync(user);        _passwordHasherMock.Setup(p => p.Verify(query.Password, hashedPassword)).Returns(true);

        // Act
        var result = await _userService.AuthenticateUserAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task AuthenticateUserAsync_ShouldReturnNull_WhenPasswordIsInvalid()
    {
        // Arrange
        var query = new AuthenticateUserQuery { Email = "test@example.com", Password = "wrong_password" };
        var hashedPassword = "hashed_password";
        var user = User.CreateNew("testuser", new Email(query.Email), hashedPassword, UserRole.Member);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email)).ReturnsAsync(user);
        _passwordHasherMock.Setup(p => p.Verify(query.Password, hashedPassword)).Returns(false);

        // Act
        var result = await _userService.AuthenticateUserAsync(query);

        // Assert
        Assert.Null(result);
    }
}
