using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Application.Sessions;

/// <summary>
/// Service for managing sessions in the system.
/// </summary>
[SingletonService]
public class SessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;

    public SessionService(
        ISessionRepository sessionRepository,
        IUserRepository userRepository)
    {
        _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Creates a new session.
    /// </summary>
    /// <param name="command">The command containing session details.</param>
    /// <returns>The created session.</returns>
    public async Task<Session> CreateSessionAsync(Commands.CreateSessionCommand command)
    {
        command.Validate();

        // Validate that the user exists
        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {command.UserId} not found.", nameof(command.UserId));
        }

        var session = Session.CreateNew(
            command.UserId,
            command.Token,
            command.ExpiresAt,
            command.IpAddress,
            command.UserAgent,
            command.RefreshToken);

        return await _sessionRepository.AddAsync(session);
    }

    /// <summary>
    /// Gets a session by its ID.
    /// </summary>
    /// <param name="query">The query containing the session ID.</param>
    /// <returns>The session if found; otherwise, null.</returns>
    public async Task<Session?> GetSessionByIdAsync(Queries.GetSessionByIdQuery query)
    {
        query.Validate();
        return await _sessionRepository.GetByIdAsync(query.Id);
    }

    /// <summary>
    /// Gets all sessions for a user.
    /// </summary>
    /// <param name="query">The query containing the user ID.</param>
    /// <returns>An array of sessions for the user.</returns>
    public async Task<Session[]> GetUserSessionsAsync(Queries.GetUserSessionsQuery query)
    {
        query.Validate();

        // Validate that the user exists
        var user = await _userRepository.GetByIdAsync(query.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {query.UserId} not found.", nameof(query.UserId));
        }

        return await _sessionRepository.GetByUserIdAsync(query.UserId);
    }

    /// <summary>
    /// Deletes a session.
    /// </summary>
    /// <param name="command">The command containing the session ID.</param>
    public async Task DeleteSessionAsync(Commands.DeleteSessionCommand command)
    {
        command.Validate();
        await _sessionRepository.DeleteAsync(command.Id);
    }
}
