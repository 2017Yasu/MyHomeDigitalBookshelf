using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

/// <summary>
/// Schema class for mapping database rows to BookshelfUser entities.
/// </summary>
public class BookshelfUserSchema
{
    public Guid UserId { get; set; }
    public Guid BookshelfId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Converts the schema to a domain entity.
    /// </summary>
    /// <returns>A BookshelfUser domain entity.</returns>
    public BookshelfUser ToEntity()
    {
        return new(
            userId: UserId,
            bookshelfId: BookshelfId,
            role: Enum.Parse<BookshelfUserRole>(Role),
            createdAt: CreatedAt,
            updatedAt: UpdatedAt);
    }
}
