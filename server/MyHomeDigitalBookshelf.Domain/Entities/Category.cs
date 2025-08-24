namespace MyHomeDigitalBookshelf.Domain.Entities;

public class Category
{
    public Guid Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public Guid BookshelfId { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public Bookshelf? Bookshelf { get; }

    public Category(
        Guid id,
        string name,
        string? description,
        Guid bookshelfId,
        DateTime createdAt,
        DateTime updatedAt,
        Bookshelf? bookshelf = null)
    {
        Id = id;
        Name = name;
        Description = description;
        BookshelfId = bookshelfId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Bookshelf = bookshelf;
    }
}
