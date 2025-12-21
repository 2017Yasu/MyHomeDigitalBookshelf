using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class UserIdentityRepositoryTests : RepositoryTestBase
{
    private readonly UserRepository _userRepository;
    private readonly UserIdentityRepository _repository;
    private int _uniqueCounter = 0;

    public UserIdentityRepositoryTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        _userRepository = new UserRepository(CreateLogger<UserRepository>(), GetConnectionProvider());
        _repository = new UserIdentityRepository(CreateLogger<UserIdentityRepository>(), GetConnectionProvider());
    }

    /// <summary>
    /// Creates a test user for use in tests.
    /// </summary>
    private async Task<User> CreateAndAddTestUserAsync()
    {
        var uniqueId = $"{++_uniqueCounter}_{Guid.NewGuid():N}".Substring(0, 12);
        var user = User.CreateNew(
            username: $"testuser_{uniqueId}",
            email: new Email($"test{uniqueId}@example.com"),
            passwordHash: "hashed_password",
            role: UserRole.Member);

        return await _userRepository.AddAsync(user);
    }

    /// <summary>
    /// Creates a test UserIdentity entity.
    /// </summary>
    private UserIdentity CreateTestUserIdentity(
        Guid userId,
        string? provider = null,
        string? subject = null,
        string? email = null)
    {
        var uniqueId = $"{++_uniqueCounter}_{Guid.NewGuid():N}".Substring(0, 12);
        var providerValue = provider ?? $"provider_{uniqueId}";
        var subjectValue = subject ?? $"subject_{uniqueId}";
        return UserIdentity.CreateNew(
            userId: userId,
            provider: providerValue,
            subject: subjectValue,
            email: string.IsNullOrEmpty(email) ? null : new Email(email));
    }

    [Fact(DisplayName = "AddAsync should insert and return a user identity")]
    public async Task AddAsync_ShouldInsertAndReturnUserIdentity()
    {
        // Arrange
        var user = await CreateAndAddTestUserAsync();
        var identity = CreateTestUserIdentity(userId: user.Id);

        // Act
        var created = await _repository.AddAsync(identity);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(created);
        Assert.NotNull(fetched);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal(identity.UserId, created.UserId);
        Assert.Equal(identity.Provider, created.Provider);
        Assert.Equal(identity.Subject, created.Subject);
        Assert.Equal(identity.Email?.Value, created.Email?.Value);
        Assert.Equal(created.Id, fetched.Id);
    }

    [Fact(DisplayName = "GetByIdAsync should return user identity by ID")]
    public async Task GetByIdAsync_ShouldReturnUserIdentityById()
    {
        // Arrange
        var user = await CreateAndAddTestUserAsync();
        var identity = CreateTestUserIdentity(userId: user.Id);
        var created = await _repository.AddAsync(identity);

        // Act
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched.Id);
        Assert.Equal(user.Id, fetched.UserId);
        Assert.Equal(identity.Provider, fetched.Provider);
        Assert.Equal(identity.Subject, fetched.Subject);
    }

    [Fact(DisplayName = "GetByIdAsync should return null for non-existent identity")]
    public async Task GetByIdAsync_ShouldReturnNullForNonExistentIdentity()
    {
        // Act
        var fetched = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(fetched);
    }

    [Fact(DisplayName = "GetByProviderAndSubjectAsync should return user identity")]
    public async Task GetByProviderAndSubjectAsync_ShouldReturnUserIdentity()
    {
        // Arrange
        var user = await CreateAndAddTestUserAsync();
        var uniqueId = $"{++_uniqueCounter}_{Guid.NewGuid():N}".Substring(0, 12);
        var identity = UserIdentity.CreateNew(
            userId: user.Id,
            provider: $"test_provider_{uniqueId}",
            subject: $"test_subject_{uniqueId}",
            email: null);
        var created = await _repository.AddAsync(identity);

        // Act
        var fetched = await _repository.GetByProviderAndSubjectAsync($"test_provider_{uniqueId}", $"test_subject_{uniqueId}");

        // Assert
        Assert.NotNull(fetched);
        Assert.Equal(user.Id, fetched.UserId);
        Assert.Equal($"test_provider_{uniqueId}", fetched.Provider);
        Assert.Equal($"test_subject_{uniqueId}", fetched.Subject);
    }

    [Fact(DisplayName = "GetByProviderAndSubjectAsync should return null for non-existent provider/subject")]
    public async Task GetByProviderAndSubjectAsync_ShouldReturnNullForNonExistent()
    {
        // Act
        var fetched = await _repository.GetByProviderAndSubjectAsync("nonexistent_provider", "nonexistent_subject");

        // Assert
        Assert.Null(fetched);
    }

    [Fact(DisplayName = "DeleteAsync should remove the user identity")]
    public async Task DeleteAsync_ShouldRemoveUserIdentity()
    {
        // Arrange
        var user = await CreateAndAddTestUserAsync();
        var identity = CreateTestUserIdentity(userId: user.Id);
        var created = await _repository.AddAsync(identity);

        // Act
        await _repository.DeleteAsync(created.Id);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.Null(fetched);
    }

    [Fact(DisplayName = "AddAsync should support optional email field")]
    public async Task AddAsync_ShouldSupportOptionalEmailField()
    {
        // Arrange
        var user = await CreateAndAddTestUserAsync();
        var identity = CreateTestUserIdentity(userId: user.Id, email: null);

        // Act
        var created = await _repository.AddAsync(identity);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(created);
        Assert.NotNull(fetched);
        Assert.Null(created.Email);
        Assert.Null(fetched.Email);
    }

    [Fact(DisplayName = "AddAsync should support user identity with email")]
    public async Task AddAsync_ShouldSupportUserIdentityWithEmail()
    {
        // Arrange
        var user = await CreateAndAddTestUserAsync();
        var identity = CreateTestUserIdentity(
            userId: user.Id,
            email: "provider@example.com");

        // Act
        var created = await _repository.AddAsync(identity);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(created);
        Assert.NotNull(fetched);
        Assert.NotNull(created.Email);
        Assert.Equal("provider@example.com", created.Email.Value);
        Assert.Equal("provider@example.com", fetched.Email?.Value);
    }

    [Fact(DisplayName = "Multiple user identities for same user should be allowed")]
    public async Task MultipleIdentitiesForSameUser_ShouldBeAllowed()
    {
        // Arrange
        var user = await CreateAndAddTestUserAsync();
        var uniqueId = $"{++_uniqueCounter}_{Guid.NewGuid():N}".Substring(0, 12);
        var identity1 = UserIdentity.CreateNew(
            userId: user.Id,
            provider: $"google_{uniqueId}",
            subject: $"google_subject_{uniqueId}",
            email: null);
        var identity2 = UserIdentity.CreateNew(
            userId: user.Id,
            provider: $"microsoft_{uniqueId}",
            subject: $"microsoft_subject_{uniqueId}",
            email: null);

        // Act
        var created1 = await _repository.AddAsync(identity1);
        var created2 = await _repository.AddAsync(identity2);

        // Assert
        Assert.NotEqual(created1.Id, created2.Id);
        Assert.Equal(user.Id, created1.UserId);
        Assert.Equal(user.Id, created2.UserId);
        Assert.Equal($"google_{uniqueId}", created1.Provider);
        Assert.Equal($"microsoft_{uniqueId}", created2.Provider);
    }

    [Fact(DisplayName = "Provider and subject uniqueness constraint should be enforced")]
    public async Task ProviderAndSubjectUniqueness_ShouldBeEnforced()
    {
        // Arrange
        var user1 = await CreateAndAddTestUserAsync();
        var user2 = await CreateAndAddTestUserAsync();

        var uniqueId = $"{++_uniqueCounter}_{Guid.NewGuid():N}".Substring(0, 12);
        var identity1 = UserIdentity.CreateNew(
            userId: user1.Id,
            provider: $"provider_unique_{uniqueId}",
            subject: $"subject_unique_{uniqueId}",
            email: null);

        var identity2 = UserIdentity.CreateNew(
            userId: user2.Id,
            provider: $"provider_unique_{uniqueId}",
            subject: $"subject_unique_{uniqueId}",
            email: null);

        // Act & Assert - Should throw due to unique constraint
        await _repository.AddAsync(identity1);
        await Assert.ThrowsAsync<Application.Common.Exceptions.DuplicateEntityException>(async () => await _repository.AddAsync(identity2));
    }

    [Fact(DisplayName = "Different providers with same subject should be allowed")]
    public async Task DifferentProviders_WithSameSubject_ShouldBeAllowed()
    {
        // Arrange
        var user1 = await CreateAndAddTestUserAsync();
        var user2 = await CreateAndAddTestUserAsync();

        var uniqueId = $"{++_uniqueCounter}_{Guid.NewGuid():N}".Substring(0, 12);
        var identity1 = UserIdentity.CreateNew(
            userId: user1.Id,
            provider: $"provider_a_{uniqueId}",
            subject: $"universal_subject_{uniqueId}",
            email: null);

        var identity2 = UserIdentity.CreateNew(
            userId: user2.Id,
            provider: $"provider_b_{uniqueId}",
            subject: $"universal_subject_{uniqueId}",
            email: null);

        // Act
        var created1 = await _repository.AddAsync(identity1);
        var created2 = await _repository.AddAsync(identity2);

        // Assert
        Assert.NotEqual(created1.Id, created2.Id);
        Assert.Equal($"provider_a_{uniqueId}", created1.Provider);
        Assert.Equal($"provider_b_{uniqueId}", created2.Provider);
        Assert.Equal($"universal_subject_{uniqueId}", created1.Subject);
        Assert.Equal($"universal_subject_{uniqueId}", created2.Subject);
    }

    [Fact(DisplayName = "Cascading delete should work when user is deleted")]
    public async Task CascadingDelete_ShouldRemoveIdentitiesWhenUserDeleted()
    {
        // Arrange
        var user = await CreateAndAddTestUserAsync();
        var identity = CreateTestUserIdentity(userId: user.Id);
        var created = await _repository.AddAsync(identity);

        // Act
        await _userRepository.DeleteAsync(user.Id);
        var fetched = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.Null(fetched);
    }
}
