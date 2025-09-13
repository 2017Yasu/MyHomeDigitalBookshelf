using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

/// <summary>
/// Repository interface for managing bookshelves in the data store.
/// </summary>
public interface IBookshelfRepository
{
    /// <summary>
    /// Retrieves a bookshelf by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bookshelf.</param>
    /// <returns>The bookshelf if found; otherwise, null.</returns>
    Task<Bookshelf?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all bookshelves in the data store.
    /// </summary>
    /// <returns>An array of all bookshelves.</returns>
    Task<Bookshelf[]> GetAllAsync();

    /// <summary>
    /// Adds a new bookshelf to the data store and returns the created bookshelf (with generated fields populated).
    /// </summary>
    /// <param name="bookshelf">The bookshelf to add.</param>
    /// <returns>The created bookshelf entity, including any generated fields (e.g., Id, timestamps).</returns>
    Task<Bookshelf> AddAsync(Bookshelf bookshelf);

    /// <summary>
    /// Updates an existing bookshelf in the data store and returns the updated bookshelf.
    /// </summary>
    /// <param name="bookshelf">The bookshelf to update.</param>
    /// <returns>The updated bookshelf entity if found; otherwise, null.</returns>
    Task<Bookshelf?> UpdateAsync(Bookshelf bookshelf);

    /// <summary>
    /// Deletes a bookshelf from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bookshelf to delete.</param>
    Task DeleteAsync(Guid id);
}
