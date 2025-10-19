using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Users.Queries;

/// <summary>
/// Query to get all users.
/// </summary>
public record GetAllUsersQuery
{
    /// <summary>
    /// No validation needed as this query has no parameters.
    /// </summary>
    public void Validate()
    {
    }
}
