namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents an external identity (OIDC, social login, etc.) linked to a user.
/// </summary>
public class UserIdentity
{
    /// <summary>
    /// Gets the unique identifier for the user identity.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the user ID to which this identity is linked.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the provider name (e.g., Google, Microsoft).
    /// </summary>
    public string Provider { get; }

    /// <summary>
    /// Gets the subject (unique identifier from the provider).
    /// </summary>
    public string Subject { get; }

    /// <summary>
    /// Gets the email address from the provider (if available).
    /// </summary>
    public ValueObjects.Email? Email { get; }

    /// <summary>
    /// Gets the timestamp when the identity was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the user entity to which this identity is linked.
    /// </summary>
    public User? User { get; }

    public UserIdentity(
        Guid id,
        Guid userId,
        string provider,
        string subject,
        ValueObjects.Email? email,
        DateTime createdAt,
        User? user = null)
        : this(userId, provider, subject, email)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id must not be empty.", nameof(id));
        if (createdAt == default)
            throw new ArgumentException("CreatedAt must be a valid date.", nameof(createdAt));

        Id = id;
        CreatedAt = createdAt;
        User = user;
    }

    public static UserIdentity CreateNew(
        Guid userId,
        string provider,
        string subject,
        ValueObjects.Email? email)
    {
        return new UserIdentity(userId, provider, subject, email);
    }

    private UserIdentity(
        Guid userId,
        string provider,
        string subject,
        ValueObjects.Email? email)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId must not be empty.", nameof(userId));
        }
        if (string.IsNullOrWhiteSpace(provider))
        {
            throw new ArgumentException("Provider must not be null or whitespace.", nameof(provider));
        }
        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new ArgumentException("Subject must not be null or whitespace.", nameof(subject));
        }

        UserId = userId;
        Provider = provider;
        Subject = subject;
        Email = email;
    }
    public override string ToString()
    {
        return Utilities.ClassUtilities.GetPropertiesInfo(this);
    }
}
