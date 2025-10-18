using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Bookshelves.Queries;

/// <summary>
/// Query to get all bookshelves.
/// </summary>
public record GetAllBookshelvesQuery
{
    /// <summary>
    /// No validation needed as this query has no parameters.
    /// </summary>
    public void Validate()
    {
    }
}
