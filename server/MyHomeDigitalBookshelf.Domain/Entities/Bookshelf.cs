namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents a bookshelf, which can contain multiple categories, books, and members.
/// </summary>
public class Bookshelf
{
    /// <summary>
    /// Gets the unique identifier for the bookshelf.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the name of the bookshelf.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the bookshelf.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Gets the timestamp when the bookshelf was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when the bookshelf was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Gets the categories contained in the bookshelf.
    /// </summary>
    public Category[] Categories { get; }

    public Bookshelf(
        Guid id,
        string name,
        string? description,
        DateTime createdAt,
        DateTime updatedAt,
        Category[]? categories = null)
        : this(name, description)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id must not be empty.", nameof(id));
        }
        if (createdAt == default)
        {
            throw new ArgumentException("CreatedAt must be a valid date.", nameof(createdAt));
        }
        if (updatedAt == default)
        {
            throw new ArgumentException("UpdatedAt must be a valid date.", nameof(updatedAt));
        }
        if (categories != null && categories.Any(c => c == null))
        {
            throw new ArgumentException("Categories array must not contain null elements.", nameof(categories));
        }

        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Categories = categories ?? [];
    }

    public static Bookshelf CreateNew(
        string name,
        string? description)
    {
        return new Bookshelf(name, description);
    }

    private Bookshelf(
        string name,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be null or whitespace.", nameof(name));
        }

        Name = name;
        Description = description;
        Categories = [];
    }

    public Bookshelf Update(
        string? name,
        string? description)
    {
        return new Bookshelf(
            Id,
            string.IsNullOrWhiteSpace(name) ? Name : name,
            string.IsNullOrWhiteSpace(description) ? Description : description,
            CreatedAt,
            DateTime.UtcNow,
            [.. Categories]);
    }

    public override string ToString()
    {
        return Utilities.ClassUtilities.GetPropertiesInfo(this);
    }
}
