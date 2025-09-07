using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

/// <summary>
/// Repository interface for managing the relationship between users and bookshelves.
/// </summary>
public interface IBookshelfUserRepository
{
    /// <summary>
    /// Retrieves the relationship between a user and a bookshelf.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="bookshelfId">The unique identifier of the bookshelf.</param>
    /// <returns>The BookshelfUser relationship if found; otherwise, null.</returns>
    Task<BookshelfUser?> GetAsync(Guid userId, Guid bookshelfId);

    /// <summary>
    /// Retrieves all user relationships for a specific bookshelf.
    /// </summary>
    /// <param name="bookshelfId">The unique identifier of the bookshelf.</param>
    /// <returns>A collection of BookshelfUser relationships for the bookshelf.</returns>
    Task<IEnumerable<BookshelfUser>> GetByBookshelfAsync(Guid bookshelfId);

    /// <summary>
    /// Retrieves all bookshelf relationships for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of BookshelfUser relationships for the user.</returns>
    Task<IEnumerable<BookshelfUser>> GetByUserAsync(Guid userId);

    /// <summary>
    /// Adds a new user-bookshelf relationship to the data store.
    /// </summary>
    /// <param name="bookshelfUser">The BookshelfUser relationship to add.</param>
    Task AddAsync(BookshelfUser bookshelfUser);

    /// <summary>
    /// Updates an existing user-bookshelf relationship in the data store.
    /// </summary>
    /// <param name="bookshelfUser">The BookshelfUser relationship to update.</param>
    Task UpdateAsync(BookshelfUser bookshelfUser);

    /// <summary>
    /// Deletes a user-bookshelf relationship from the data store.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="bookshelfId">The unique identifier of the bookshelf.</param>
    Task DeleteAsync(Guid userId, Guid bookshelfId);
}
