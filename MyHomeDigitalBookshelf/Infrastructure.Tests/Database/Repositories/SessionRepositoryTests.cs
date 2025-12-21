using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class SessionRepositoryTests : RepositoryTestBase
{
    private readonly SessionRepository _repository;
    private readonly UserRepository _userRepository;

    public SessionRepositoryTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        _repository = new SessionRepository(CreateLogger<SessionRepository>(), GetConnectionProvider());
        _userRepository = new UserRepository(CreateLogger<UserRepository>(), GetConnectionProvider());
    }

    private async Task<User> CreateTestUser(string? username = null, string? email = null)
    {
        // Generate unique username and email if not provided
        var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
        var user = User.CreateNew(
            username: username ?? $"test_user_{uniqueId}",
            email: new Email(email ?? $"test{uniqueId}@example.com"),
            passwordHash: "hashed_password",
            role: UserRole.Member);
        return await _userRepository.AddAsync(user);
    }

    [Fact(DisplayName = "AddAsync should insert and return a session")]
    public async Task AddAsync_ShouldInsertAndReturnSession()
    {
        // Arrange
        var user = await CreateTestUser();
        var session = Session.CreateNew(
            userId: user.Id,
            token: "test_token",
            expiresAt: DateTime.UtcNow.AddHours(1),
            ipAddress: "127.0.0.1",
            userAgent: "test_agent",
            refreshToken: "test_refresh_token");

        // Act
        var created = await _repository.AddAsync(session);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(created);
        Assert.NotNull(fetched);
        Assert.Equal(session.Token, created.Token);
        Assert.Equal(session.UserId, created.UserId);
        Assert.Equal(session.IpAddress, created.IpAddress);
        Assert.Equal(session.UserAgent, created.UserAgent);
        Assert.Equal(session.RefreshToken, created.RefreshToken);
        Assert.Equal(created.Id, fetched.Id);
    }

    [Fact(DisplayName = "GetByUserIdAsync should return all sessions for a user")]
    public async Task GetByUserIdAsync_ShouldReturnAllSessionsForUser()
    {
        // Arrange
        var user = await CreateTestUser();
        var session1 = Session.CreateNew(
            userId: user.Id,
            token: "test_token_1",
            expiresAt: DateTime.UtcNow.AddHours(1));
        var session2 = Session.CreateNew(
            userId: user.Id,
            token: "test_token_2",
            expiresAt: DateTime.UtcNow.AddHours(2));

        await _repository.AddAsync(session1);
        await _repository.AddAsync(session2);

        // Act
        var sessions = await _repository.GetByUserIdAsync(user.Id);

        // Assert
        Assert.Equal(2, sessions.Length);
        Assert.All(sessions, s => Assert.Equal(user.Id, s.UserId));
    }

    [Fact(DisplayName = "DeleteAsync should remove the session")]
    public async Task DeleteAsync_ShouldRemoveSession()
    {
        // Arrange
        var user = await CreateTestUser();
        var session = Session.CreateNew(
            userId: user.Id,
            token: "test_token",
            expiresAt: DateTime.UtcNow.AddHours(1));
        var created = await _repository.AddAsync(session);

        // Act
        await _repository.DeleteAsync(created.Id);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.Null(fetched);
    }

    [Fact(DisplayName = "DeleteExpiredAsync should remove expired sessions")]
    public async Task DeleteExpiredAsync_ShouldRemoveExpiredSessions()
    {
        // Arrange
        var user = await CreateTestUser();
        var sessionExpired = Session.CreateNew(
            userId: user.Id,
            token: "expired_token",
            expiresAt: DateTime.UtcNow.AddHours(-1));
        var sessionValid = Session.CreateNew(
            userId: user.Id,
            token: "valid_token",
            expiresAt: DateTime.UtcNow.AddHours(1));
        await _repository.AddAsync(sessionExpired);
        await _repository.AddAsync(sessionValid);

        // Act
        var deletedCount = await _repository.DeleteExpiredAsync();
        var remainingSessions = await _repository.GetByUserIdAsync(user.Id);

        // Assert
        Assert.Equal(1, deletedCount);
        Assert.Single(remainingSessions);
        Assert.Equal("valid_token", remainingSessions[0].Token);
    }
}
