using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

/// <summary>
/// Repository interface for managing user sessions in the data store.
/// </summary>
public interface ISessionRepository
{
    /// <summary>
    /// Retrieves a session by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the session.</param>
    /// <returns>The session if found; otherwise, null.</returns>
    Task<Session?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all sessions for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of sessions for the user.</returns>
    Task<IEnumerable<Session>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Adds a new session to the data store.
    /// </summary>
    /// <param name="session">The session to add.</param>
    Task AddAsync(Session session);

    /// <summary>
    /// Deletes a session from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the session to delete.</param>
    Task DeleteAsync(Guid id);
}
