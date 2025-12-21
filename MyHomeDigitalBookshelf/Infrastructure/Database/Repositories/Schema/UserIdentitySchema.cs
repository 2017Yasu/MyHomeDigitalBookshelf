using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

/// <summary>
/// Schema class for mapping database rows to UserIdentity entities.
/// </summary>
public class UserIdentitySchema
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Converts the schema to a domain entity.
    /// </summary>
    /// <returns>A UserIdentity domain entity.</returns>
    public Domain.Entities.UserIdentity ToEntity()
    {
        return new(
            id: Id,
            userId: UserId,
            provider: Provider,
            subject: Subject,
            email: string.IsNullOrEmpty(Email) ? null : new Email(Email),
            createdAt: CreatedAt);
    }
}
