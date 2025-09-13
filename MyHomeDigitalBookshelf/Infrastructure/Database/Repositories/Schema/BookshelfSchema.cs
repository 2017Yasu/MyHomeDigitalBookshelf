namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

public class BookshelfSchema
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Domain.Entities.Bookshelf ToEntity()
    {
        return new Domain.Entities.Bookshelf(
            id: this.Id,
            name: this.Name,
            description: this.Description,
            createdAt: this.CreatedAt,
            updatedAt: this.UpdatedAt);
    }
}
