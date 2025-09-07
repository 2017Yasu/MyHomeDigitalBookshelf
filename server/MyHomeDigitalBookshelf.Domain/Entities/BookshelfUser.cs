namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents the relationship between a user and a bookshelf, including the user's role.
/// </summary>
public class BookshelfUser
{
    /// <summary>
    /// Gets the user ID in the relationship.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the bookshelf ID in the relationship.
    /// </summary>
    public Guid BookshelfId { get; }

    /// <summary>
    /// Gets the role of the user in the bookshelf.
    /// </summary>
    public BookshelfUserRole Role { get; }

    /// <summary>
    /// Gets the timestamp when the relationship was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when the relationship was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Gets the user entity in the relationship.
    /// </summary>
    public User? User { get; }

    /// <summary>
    /// Gets the bookshelf entity in the relationship.
    /// </summary>
    public Bookshelf? Bookshelf { get; }

    public BookshelfUser(
        Guid userId,
        Guid bookshelfId,
        BookshelfUserRole role,
        DateTime createdAt,
        DateTime updatedAt,
        User? user = null,
        Bookshelf? bookshelf = null)
        : this(userId, bookshelfId, role)
    {
        if (createdAt == default)
        {
            throw new ArgumentException("CreatedAt must be a valid date.", nameof(createdAt));
        }
        if (updatedAt == default)
        {
            throw new ArgumentException("UpdatedAt must be a valid date.", nameof(updatedAt));
        }

        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        User = user;
        Bookshelf = bookshelf;
    }

    public static BookshelfUser CreateNew(
        Guid userId,
        Guid bookshelfId,
        BookshelfUserRole role)
    {
        return new BookshelfUser(userId, bookshelfId, role);
    }

    private BookshelfUser(
        Guid userId,
        Guid bookshelfId,
        BookshelfUserRole role)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId must not be empty.", nameof(userId));
        }
        if (bookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId must not be empty.", nameof(bookshelfId));
        }

        UserId = userId;
        BookshelfId = bookshelfId;
        Role = role;
    }
}
