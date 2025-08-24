namespace MyHomeDigitalBookshelf.Domain.Entities;

public class UserIdentity
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public string Provider { get; }
    public string Subject { get; }
    public ValueObjects.Email? Email { get; }
    public DateTime CreatedAt { get; }
    public User? User { get; }

    public UserIdentity(
        Guid id,
        Guid userId,
        string provider,
        string subject,
        ValueObjects.Email? email,
        DateTime createdAt,
        User? user = null)
    {
        Id = id;
        UserId = userId;
        Provider = provider;
        Subject = subject;
        Email = email;
        CreatedAt = createdAt;
        User = user;
    }
}
