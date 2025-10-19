using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.Users.Commands;

/// <summary>
/// Command to add a new user.
/// </summary>
public record AddUserCommand(
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
        if (string.IsNullOrWhiteSpace(Username))
        {
            throw new ArgumentException("Username is required.", nameof(Username));
        }
    }
}
