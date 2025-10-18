using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.Users.Commands;

/// <summary>
/// Command to add a user identity.
/// </summary>
public record AddUserIdentityCommand(
    Guid UserId,
    string Provider,
    string Subject,
    string? Email)
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
        if (string.IsNullOrWhiteSpace(Provider))
        {
            throw new ArgumentException("Provider is required.", nameof(Provider));
        }
        if (string.IsNullOrWhiteSpace(Subject))
        {
            throw new ArgumentException("Subject is required.", nameof(Subject));
        }
    }
}
