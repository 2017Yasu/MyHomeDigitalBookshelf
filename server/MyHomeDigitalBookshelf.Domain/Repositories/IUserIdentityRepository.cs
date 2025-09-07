using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

/// <summary>
/// Repository interface for managing user identities (external logins) in the data store.
/// </summary>
public interface IUserIdentityRepository
{
    /// <summary>
    /// Retrieves a user identity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user identity.</param>
    /// <returns>The user identity if found; otherwise, null.</returns>
    Task<UserIdentity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user identity by provider and subject.
    /// </summary>
    /// <param name="provider">The authentication provider (e.g., Google, Facebook).</param>
    /// <param name="subject">The subject identifier from the provider.</param>
    /// <returns>The user identity if found; otherwise, null.</returns>
    Task<UserIdentity?> GetByProviderAndSubjectAsync(string provider, string subject);

    /// <summary>
    /// Adds a new user identity to the data store and returns the created identity (with generated fields populated).
    /// </summary>
    /// <param name="identity">The user identity to add.</param>
    /// <returns>The created UserIdentity entity, including any generated fields (e.g., Id, timestamps).</returns>
    Task<UserIdentity> AddAsync(UserIdentity identity);

    /// <summary>
    /// Deletes a user identity from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user identity to delete.</param>
    Task DeleteAsync(Guid id);
}
