using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

/// <summary>
/// Repository interface for managing users in the data store.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Retrieves all users in the data store.
    /// </summary>
    /// <returns>An array of all users.</returns>
    Task<User[]> GetAllAsync();

    /// <summary>
    /// Adds a new user to the data store and returns the created user (with generated fields populated).
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <returns>The created user entity, including any generated fields (e.g., Id, timestamps).</returns>
    Task<User> AddAsync(User user);

    /// <summary>
    /// Updates an existing user in the data store and returns the updated user.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>The updated user entity if found; otherwise, null.</returns>
    Task<User?> UpdateAsync(User user);

    /// <summary>
    /// Deletes a user from the data store by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    Task DeleteAsync(Guid id);
}
