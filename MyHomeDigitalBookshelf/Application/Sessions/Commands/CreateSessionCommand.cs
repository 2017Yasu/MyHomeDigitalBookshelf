using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Sessions.Commands;

/// <summary>
/// Command to create a new session.
/// </summary>
public record CreateSessionCommand(
    Guid UserId,
    string Token,
    DateTime ExpiresAt,
    string? IpAddress = null,
    string? UserAgent = null,
    string? RefreshToken = null)
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
        if (string.IsNullOrWhiteSpace(Token))
        {
            throw new ArgumentException("Token is required.", nameof(Token));
        }
        if (ExpiresAt <= DateTime.UtcNow)
        {
            throw new ArgumentException("ExpiresAt must be in the future.", nameof(ExpiresAt));
        }
    }
}
