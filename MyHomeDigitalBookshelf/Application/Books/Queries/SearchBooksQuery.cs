using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.Books.Queries;

/// <summary>
/// Query to search for books based on various criteria.
/// </summary>
public record SearchBooksQuery(
    string? Title = null,
    string? Author = null,
    string? Isbn = null,
    Guid? CategoryId = null,
    CCode? CCode = null,
    Guid? OwnerId = null,
    ReadingStatus? ReadingStatus = null)
{
    /// <summary>
    /// Validates the query parameters.
    /// </summary>
    public void Validate()
    {
        // At least one search criterion should be provided
        if (Title == null && Author == null && Isbn == null && CategoryId == null && CCode == null && OwnerId == null && ReadingStatus == null)
        {
            throw new ArgumentException("At least one search criterion must be provided.");
        }
    }
}
