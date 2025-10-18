using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Users.Commands;

/// <summary>
/// Command to delete a user.
/// </summary>
public record DeleteUserCommand(Guid Id)
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
    }
}
