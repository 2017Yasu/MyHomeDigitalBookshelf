namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents a user of the digital bookshelf system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the username of the user.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public ValueObjects.Email? Email { get; }

    /// <summary>
    /// Gets the password hash for the user (if using local login).
    /// </summary>
    public string? PasswordHash { get; }

    /// <summary>
    /// Gets the role of the user in the system.
    /// </summary>
    public UserRole Role { get; }

    /// <summary>
    /// Gets the timestamp when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    public User(
        Guid id,
        string username,
        ValueObjects.Email? email,
        string? passwordHash,
        UserRole role,
        DateTime createdAt)
        : this(username, email, passwordHash, role)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id must not be empty.", nameof(id));
        }
        if (createdAt == default)
        {
            throw new ArgumentException("CreatedAt must be a valid date.", nameof(createdAt));
        }

        Id = id;
        CreatedAt = createdAt;
    }

    public static User CreateNew(
        string username,
        ValueObjects.Email? email,
        string? passwordHash,
        UserRole role)
    {
        return new User(username, email, passwordHash, role);
    }

    private User(
        string username,
        ValueObjects.Email? email,
        string? passwordHash,
        UserRole role)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username must not be null or whitespace.", nameof(username));
        }

        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
    public override string ToString()
    {
        return Utilities.ClassUtilities.GetPropertiesInfo(this);
    }
}
