namespace MyHomeDigitalBookshelf.Domain.Entities;

public enum UserRole
{
    Administrator,
    Member,
    Guest
}

public class User
{
    public Guid Id { get; }
    public string Username { get; }
    public ValueObjects.Email? Email { get; }
    public string? PasswordHash { get; }
    public UserRole Role { get; }
    public DateTime CreatedAt { get; }
    public UserIdentity[] Identities { get; }
    public BookshelfUser[] BookshelfMemberships { get; }
    public UserBook[] UserBooks { get; }

    public User(
        Guid id,
        string username,
        ValueObjects.Email? email,
        string? passwordHash,
        UserRole role,
        DateTime createdAt,
        UserIdentity[]? identities = null,
        BookshelfUser[]? bookshelfMemberships = null,
        UserBook[]? userBooks = null)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = createdAt;
        Identities = identities ?? Array.Empty<UserIdentity>();
        BookshelfMemberships = bookshelfMemberships ?? Array.Empty<BookshelfUser>();
        UserBooks = userBooks ?? Array.Empty<UserBook>();
    }
}
