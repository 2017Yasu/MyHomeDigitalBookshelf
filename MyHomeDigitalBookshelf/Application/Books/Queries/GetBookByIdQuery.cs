using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Books.Queries;

/// <summary>
/// Query to get a book by its ID.
/// </summary>
public record GetBookByIdQuery(Guid Id)
{
    /// <summary>
    /// Validates the query parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are invalid.</exception>
    public void Validate()
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("Id is required.", nameof(Id));
        }
    }
}
