using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

/// <summary>
/// Schema class for mapping database rows to User entities.
/// </summary>
public class UserSchema
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Converts the schema to a domain entity.
    /// </summary>
    /// <returns>A User domain entity.</returns>
    public Domain.Entities.User ToEntity()
    {
        return new(
            id: Id,
            username: Username,
            email: string.IsNullOrEmpty(Email) ? null : new Email(Email),
            passwordHash: PasswordHash,
            role: Enum.Parse<UserRole>(Role),
            createdAt: CreatedAt);
    }
}
