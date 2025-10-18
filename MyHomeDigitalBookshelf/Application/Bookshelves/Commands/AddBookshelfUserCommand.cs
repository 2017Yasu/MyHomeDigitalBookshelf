using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Bookshelves.Commands;

/// <summary>
/// Command to add a user to a bookshelf.
/// </summary>
public record AddBookshelfUserCommand(
    Guid UserId,
    Guid BookshelfId,
    BookshelfUserRole Role)
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
