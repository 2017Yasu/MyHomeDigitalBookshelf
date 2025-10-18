namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents a user session, including authentication tokens and metadata.
/// </summary>
public class Session
{
    /// <summary>
    /// Gets the unique identifier for the session.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the user ID associated with the session.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the authentication token for the session.
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// Gets the timestamp when the session was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when the session expires.
    /// </summary>
    public DateTime ExpiresAt { get; }

    /// <summary>
    /// Gets the originating IP address for the session (if available).
    /// </summary>
    public string? IpAddress { get; }

    /// <summary>
    /// Gets the user agent string for the session (if available).
    /// </summary>
    public string? UserAgent { get; }

    /// <summary>
    /// Gets the refresh token for the session (if available).
    /// </summary>
    public string? RefreshToken { get; }

    /// <summary>
    /// Gets the user entity associated with the session.
    /// </summary>
    public User? User { get; }

    public Session(
        Guid id,
        Guid userId,
        string token,
        DateTime createdAt,
        DateTime expiresAt,
        string? ipAddress = null,
        string? userAgent = null,
        string? refreshToken = null,
        User? user = null)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id must not be empty.", nameof(id));
        }
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId must not be empty.", nameof(userId));
        }
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token must not be null or whitespace.", nameof(token));
        }
        if (createdAt == default)
        {
            throw new ArgumentException("CreatedAt must be a valid date.", nameof(createdAt));
        }
        if (expiresAt == default)
        {
            throw new ArgumentException("ExpiresAt must be a valid date.", nameof(expiresAt));
        }

        Id = id;
        UserId = userId;
        Token = token;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        RefreshToken = refreshToken;
        User = user;
    }
    /// <summary>
    /// Creates a new session with the specified parameters.
    /// </summary>
    /// <param name="userId">The ID of the user for the session.</param>
    /// <param name="token">The authentication token.</param>
    /// <param name="expiresAt">The expiration time for the session.</param>
    /// <param name="ipAddress">Optional. The IP address from which the session was created.</param>
    /// <param name="userAgent">Optional. The user agent string from which the session was created.</param>
    /// <param name="refreshToken">Optional. The refresh token for the session.</param>
    /// <param name="user">Optional. The user entity associated with the session.</param>
    /// <returns>A new session instance.</returns>
    public static Session CreateNew(
        Guid userId,
        string token,
        DateTime expiresAt,
        string? ipAddress = null,
        string? userAgent = null,
        string? refreshToken = null,
        User? user = null)
    {
        return new Session(
            Guid.NewGuid(),
            userId,
            token,
            DateTime.UtcNow,
            expiresAt,
            ipAddress,
            userAgent,
            refreshToken,
            user);
    }

    public override string ToString()
    {
        return Utilities.ClassUtilities.GetPropertiesInfo(this);
    }
}
