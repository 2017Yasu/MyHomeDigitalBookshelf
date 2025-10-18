using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Bookshelves.Queries;

/// <summary>
/// Query to get all bookshelves a user belongs to.
/// </summary>
public record GetUserBookshelvesQuery(Guid UserId)
{
    /// <summary>
    /// Validates the query parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are invalid.</exception>
    public void Validate()
    {
        if (UserId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(UserId));
        }
    }
}
