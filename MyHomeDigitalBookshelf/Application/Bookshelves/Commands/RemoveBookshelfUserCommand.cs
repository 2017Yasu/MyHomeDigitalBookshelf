using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Bookshelves.Commands;

/// <summary>
/// Command to remove a user from a bookshelf.
/// </summary>
public record RemoveBookshelfUserCommand(
    Guid UserId,
    Guid BookshelfId)
{
    /// <summary>
    /// Validates the command parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are invalid.</exception>
    public void Validate()
    {
        if (UserId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(UserId));
        }
        if (BookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId is required.", nameof(BookshelfId));
        }
    }
}
