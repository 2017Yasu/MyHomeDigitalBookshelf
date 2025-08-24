namespace MyHomeDigitalBookshelf.Domain.Entities;

public class Bookshelf
{
    public Guid Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public Category[] Categories { get; }
    public Book[] Books { get; }
    public BookshelfUser[] Members { get; }

    public Bookshelf(
        Guid id,
        string name,
        string? description,
        DateTime createdAt,
        DateTime updatedAt,
        Category[]? categories = null,
        Book[]? books = null,
        BookshelfUser[]? members = null)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Categories = categories ?? Array.Empty<Category>();
        Books = books ?? Array.Empty<Book>();
        Members = members ?? Array.Empty<BookshelfUser>();
    }
}
