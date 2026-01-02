using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Books.Interfaces;

public interface IBookFinderService
{
    /// <summary>
    /// Finds a book by its ISBN using an external service like Google Books API.
    /// </summary>
    /// <param name="isbn">The ISBN of the book to find.</param>
    /// <returns>A Book entity if found; otherwise, null.</returns>
    Task<Book?> FindByIsbnAsync(string isbn);
}
