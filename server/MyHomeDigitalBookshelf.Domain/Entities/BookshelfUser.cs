namespace MyHomeDigitalBookshelf.Domain.Entities;

public enum BookshelfUserRole
{
    Administrator,
    Member,
    Guest
}

public class BookshelfUser
{
    public Guid UserId { get; }
    public Guid BookshelfId { get; }
    public BookshelfUserRole Role { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public User? User { get; }
    public Bookshelf? Bookshelf { get; }

    public BookshelfUser(
        Guid userId,
        Guid bookshelfId,
        BookshelfUserRole role,
        DateTime createdAt,
        DateTime updatedAt,
        User? user = null,
        Bookshelf? bookshelf = null)
    {
        UserId = userId;
        BookshelfId = bookshelfId;
        Role = role;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        User = user;
        Bookshelf = bookshelf;
    }
}
