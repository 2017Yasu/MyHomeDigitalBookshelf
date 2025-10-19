namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

public class CategorySchema
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid BookshelfId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Domain.Entities.Category ToEntity()
    {
        return new Domain.Entities.Category(
            id: this.Id,
            name: this.Name,
            description: this.Description,
            bookshelfId: this.BookshelfId,
            createdAt: this.CreatedAt,
            updatedAt: this.UpdatedAt);
    }
}
