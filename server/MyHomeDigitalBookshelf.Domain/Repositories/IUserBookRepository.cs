using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

/// <summary>
/// Repository interface for managing the relationship between users and books in the data store.
/// </summary>
public interface IUserBookRepository
{
    /// <summary>
    /// Retrieves the relationship between a user and a book.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="bookId">The unique identifier of the book.</param>
    /// <returns>The UserBook relationship if found; otherwise, null.</returns>
    Task<UserBook?> GetAsync(Guid userId, Guid bookId);

    /// <summary>
    /// Retrieves all user-book relationships for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of UserBook relationships for the user.</returns>
    Task<IEnumerable<UserBook>> GetByUserAsync(Guid userId);

    /// <summary>
    /// Retrieves all user-book relationships for a specific book.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book.</param>
    /// <returns>A collection of UserBook relationships for the book.</returns>
    Task<IEnumerable<UserBook>> GetByBookAsync(Guid bookId);

    /// <summary>
    /// Adds a new user-book relationship to the data store.
    /// </summary>
    /// <param name="userBook">The UserBook relationship to add.</param>
    Task AddAsync(UserBook userBook);

    /// <summary>
    /// Updates an existing user-book relationship in the data store.
    /// </summary>
    /// <param name="userBook">The UserBook relationship to update.</param>
    Task UpdateAsync(UserBook userBook);

    /// <summary>
    /// Deletes a user-book relationship from the data store.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="bookId">The unique identifier of the book.</param>
    Task DeleteAsync(Guid userId, Guid bookId);
}
