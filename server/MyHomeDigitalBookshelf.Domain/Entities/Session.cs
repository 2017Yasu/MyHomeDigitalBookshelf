namespace MyHomeDigitalBookshelf.Domain.Entities;

public class Session
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public string Token { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }
    public string? IpAddress { get; }
    public string? UserAgent { get; }
    public string? RefreshToken { get; }
    public User? User { get; }

    public Session(
        Guid id,
        Guid userId,
        string token,
        DateTime createdAt,
        DateTime expiresAt,
        string? ipAddress = null,
        string? userAgent = null,
        string? refreshToken = null,
        User? user = null)
    {
        Id = id;
        UserId = userId;
        Token = token;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        RefreshToken = refreshToken;
        User = user;
    }
}
