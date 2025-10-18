using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Sessions.Queries;

/// <summary>
/// Query to get all sessions for a user.
/// </summary>
public record GetUserSessionsQuery(Guid UserId)
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
