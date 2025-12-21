using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class UserRepositoryTests : RepositoryTestBase
{
    private readonly UserRepository _repository;
    private int _uniqueCounter = 0;

    public UserRepositoryTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        _repository = new UserRepository(CreateLogger<UserRepository>(), GetConnectionProvider());
    }

    private User CreateTestUser(
        string? username = null,
        string? email = null,
        string? passwordHash = "hashed_password",
        UserRole? role = UserRole.Member)
    {
        var uniqueId = $"{++_uniqueCounter}_{Guid.NewGuid():N}".Substring(0, 12);
        return User.CreateNew(
            username: username ?? $"testuser_{uniqueId}",
            email: new Email(email ?? $"test{uniqueId}@example.com"),
            passwordHash: passwordHash,
            role: role ?? UserRole.Member);
    }

    [Fact(DisplayName = "AddAsync should insert and return a user")]
    public async Task AddAsync_ShouldInsertAndReturnUser()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        var created = await _repository.AddAsync(user);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(created);
        Assert.NotNull(fetched);
        Assert.Equal(user.Username, created.Username);
        Assert.Equal(user.Email?.Value, created.Email?.Value);
        Assert.Equal(user.PasswordHash, created.PasswordHash);
        Assert.Equal(user.Role, created.Role);
        Assert.Equal(created.Id, fetched.Id);
    }

    [Fact(DisplayName = "GetByUsernameAsync should return user")]
    public async Task GetByUsernameAsync_ShouldReturnUser()
    {
        // Arrange
        var user = CreateTestUser();
        await _repository.AddAsync(user);

        // Act
        var fetched = await _repository.GetByUsernameAsync(user.Username);

        // Assert
        Assert.NotNull(fetched);
        Assert.Equal(user.Username, fetched.Username);
        Assert.Equal(user.Email?.Value, fetched.Email?.Value);
    }

    [Fact(DisplayName = "GetByEmailAsync should return user")]
    public async Task GetByEmailAsync_ShouldReturnUser()
    {
        // Arrange
        var user = CreateTestUser();
        await _repository.AddAsync(user);

        // Act
        var fetched = await _repository.GetByEmailAsync(user.Email!.Value);

        // Assert
        Assert.NotNull(fetched);
        Assert.Equal(user.Username, fetched.Username);
        Assert.Equal(user.Email?.Value, fetched.Email?.Value);
    }

    [Fact(DisplayName = "UpdateAsync should update and return user")]
    public async Task UpdateAsync_ShouldUpdateAndReturnUser()
    {
        // Arrange
        var user = CreateTestUser(passwordHash: "original_hash");
        var created = await _repository.AddAsync(user);

        var updated = CreateTestUser(passwordHash: "new_hash", role: UserRole.Administrator);
        // Use constructor to preserve the original ID
        updated = new User(
            id: created.Id,
            username: updated.Username,
            email: updated.Email,
            passwordHash: updated.PasswordHash,
            role: updated.Role,
            createdAt: created.CreatedAt);

        // Act
        var result = await _repository.UpdateAsync(updated);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(fetched);
        Assert.Equal(updated.Username, result.Username);
        Assert.Equal(updated.Email?.Value, result.Email?.Value);
        Assert.Equal(updated.PasswordHash, result.PasswordHash);
        Assert.Equal(updated.Role, result.Role);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.CreatedAt, result.CreatedAt);
    }

    [Fact(DisplayName = "DeleteAsync should remove the user")]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        // Arrange
        var user = CreateTestUser();
        var created = await _repository.AddAsync(user);

        // Act
        await _repository.DeleteAsync(created.Id);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.Null(fetched);
    }

    [Fact(DisplayName = "GetAllAsync should return all users")]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var user1 = CreateTestUser(passwordHash: "hashed_password1");
        var user2 = CreateTestUser(passwordHash: "hashed_password2", role: UserRole.Administrator);

        await _repository.AddAsync(user1);
        await _repository.AddAsync(user2);

        // Act
        var users = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(users);
        Assert.Contains(users, u => u.Username == user1.Username);
        Assert.Contains(users, u => u.Username == user2.Username);
    }
}
