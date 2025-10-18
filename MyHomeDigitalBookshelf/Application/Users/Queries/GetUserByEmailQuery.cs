using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Users.Queries;

/// <summary>
/// Query to get a user by their email.
/// </summary>
public record GetUserByEmailQuery(string Email)
{
    /// <summary>
    /// Validates the query parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            throw new ArgumentException("Email is required.", nameof(Email));
        }
    }
}
