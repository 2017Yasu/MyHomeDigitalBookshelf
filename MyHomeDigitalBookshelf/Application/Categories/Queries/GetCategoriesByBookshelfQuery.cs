using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Categories.Queries;

/// <summary>
/// Query to get all categories for a bookshelf.
/// </summary>
public record GetCategoriesByBookshelfQuery(Guid BookshelfId)
{
    /// <summary>
    /// Validates the query parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are invalid.</exception>
    public void Validate()
    {
        if (BookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId is required.", nameof(BookshelfId));
        }
    }
}
