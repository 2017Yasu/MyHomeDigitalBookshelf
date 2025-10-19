using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.Books.Commands;

/// <summary>
/// Command to update an existing book.
/// </summary>
public record UpdateBookCommand(
    Guid Id,
    string Title,
    Guid BookshelfId,
    string[]? Authors = null,
    Isbn? Isbn = null,
    string? Publisher = null,
    DateTime? PublishDate = null,
    CCode? CCode = null,
    Guid? CategoryId = null,
    string? CoverImageUrl = null,
    string? Notes = null)
{
    /// <summary>
    /// Validates the command parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are invalid.</exception>
    public void Validate()
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("Id is required.", nameof(Id));
        }
        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new ArgumentException("Title is required.", nameof(Title));
        }
        if (BookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId is required.", nameof(BookshelfId));
        }
    }
}
