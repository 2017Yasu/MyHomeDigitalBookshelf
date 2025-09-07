namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents a category for organizing books within a bookshelf.
/// </summary>
public class Category
{
    /// <summary>
    /// Gets the unique identifier for the category.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the name of the category.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the category.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Gets the ID of the bookshelf to which the category belongs.
    /// </summary>
    public Guid BookshelfId { get; }

    /// <summary>
    /// Gets the timestamp when the category was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when the category was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; }

    public Category(
        Guid id,
        string name,
        string? description,
        Guid bookshelfId,
        DateTime createdAt,
        DateTime updatedAt)
        : this(name, description, bookshelfId)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id must not be empty.", nameof(id));
        if (createdAt == default)
            throw new ArgumentException("CreatedAt must be a valid date.", nameof(createdAt));
        if (updatedAt == default)
            throw new ArgumentException("UpdatedAt must be a valid date.", nameof(updatedAt));

        Id = id;
        BookshelfId = bookshelfId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Category CreateNew(
        string name,
        string? description,
        Guid bookshelfId)
    {
        return new Category(name, description, bookshelfId);
    }

    private Category(
        string name,
        string? description,
        Guid bookshelfId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name must not be null or whitespace.", nameof(name));
        if (bookshelfId == Guid.Empty)
            throw new ArgumentException("BookshelfId must not be empty.", nameof(bookshelfId));

        Name = name;
        Description = description;
        BookshelfId = bookshelfId;
    }
}
