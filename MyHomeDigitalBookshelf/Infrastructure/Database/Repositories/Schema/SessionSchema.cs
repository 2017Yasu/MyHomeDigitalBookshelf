namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

/// <summary>
/// Schema class for mapping database rows to Session entities.
/// </summary>
public class SessionSchema
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Converts the schema to a domain entity.
    /// </summary>
    /// <returns>A Session domain entity.</returns>
    public Domain.Entities.Session ToEntity()
    {
        return new(
            id: Id,
            userId: UserId,
            token: Token,
            createdAt: CreatedAt,
            expiresAt: ExpiresAt,
            ipAddress: IpAddress,
            userAgent: UserAgent,
            refreshToken: RefreshToken);
    }
}
