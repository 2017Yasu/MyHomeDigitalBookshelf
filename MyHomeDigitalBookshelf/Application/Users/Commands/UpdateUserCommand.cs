using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.Users.Commands;

/// <summary>
/// Command to update an existing user.
/// </summary>
public record UpdateUserCommand(
    Guid Id,
    string Username,
    Email? Email,
    string? PasswordHash,
    UserRole Role)
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
        if (string.IsNullOrWhiteSpace(Username))
        {
            throw new ArgumentException("Username is required.", nameof(Username));
        }
    }
}
