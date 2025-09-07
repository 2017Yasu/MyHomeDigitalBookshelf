using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

/// <summary>
/// Repository interface for managing books in the data store.
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Retrieves a book by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the book.</param>
    /// <returns>The book if found; otherwise, null.</returns>
    Task<Book?> GetByIdAsync(Guid id);

    /// <summary>
    /// Searches for books matching the specified criteria.
    /// </summary>
    /// <param name="title">The title of the book (optional).</param>
    /// <param name="author">The author of the book (optional).</param>
    /// <param name="isbn">The ISBN of the book (optional).</param>
    /// <param name="categoryId">The category ID of the book (optional).</param>
    /// <param name="cCode">The CCode of the book (optional).</param>
    /// <param name="ownerId">The owner ID of the book (optional).</param>
    /// <param name="readingStatus">The reading status of the book (optional).</param>
    /// <returns>A collection of books matching the search criteria.</returns>
    Task<IEnumerable<Book>> SearchAsync(string? title, string? author, string? isbn, Guid? categoryId, string? cCode, Guid? ownerId, ReadingStatus? readingStatus);

    /// <summary>
    /// Adds a new book to the data store.
    /// </summary>
    /// <param name="book">The book to add.</param>
    Task AddAsync(Book book);

    /// <summary>
    /// Updates an existing book in the data store.
    /// </summary>
    /// <param name="book">The book to update.</param>
    Task UpdateAsync(Book book);

    /// <summary>
    /// Deletes a book from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the book to delete.</param>
    Task DeleteAsync(Guid id);
}
