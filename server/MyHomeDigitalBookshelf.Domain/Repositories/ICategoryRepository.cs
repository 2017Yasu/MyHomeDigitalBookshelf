using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

/// <summary>
/// Repository interface for managing categories in the data store.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Retrieves a category by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    Task<Category?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all categories for a specific bookshelf.
    /// </summary>
    /// <param name="bookshelfId">The unique identifier of the bookshelf.</param>
    /// <returns>A collection of categories for the bookshelf.</returns>
    Task<IEnumerable<Category>> GetAllByBookshelfAsync(Guid bookshelfId);

    /// <summary>
    /// Adds a new category to the data store.
    /// </summary>
    /// <param name="category">The category to add.</param>
    Task AddAsync(Category category);

    /// <summary>
    /// Updates an existing category in the data store.
    /// </summary>
    /// <param name="category">The category to update.</param>
    Task UpdateAsync(Category category);

    /// <summary>
    /// Deletes a category from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category to delete.</param>
    Task DeleteAsync(Guid id);
}
