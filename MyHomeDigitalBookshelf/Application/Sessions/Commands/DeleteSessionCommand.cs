using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Sessions.Commands;

/// <summary>
/// Command to delete a session.
/// </summary>
public record DeleteSessionCommand(Guid Id)
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
