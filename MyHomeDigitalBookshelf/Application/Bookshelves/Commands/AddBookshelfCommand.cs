using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Bookshelves.Commands;

/// <summary>
/// Command to add a new bookshelf.
/// </summary>
public record AddBookshelfCommand(
    string Name,
    string? Description = null)
{
    /// <summary>
    /// Validates the command parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Name is required.", nameof(Name));
        }
    }
}
