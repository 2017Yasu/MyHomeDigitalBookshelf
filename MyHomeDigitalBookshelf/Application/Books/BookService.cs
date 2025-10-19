using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Application.Books;

/// <summary>
/// Service for managing books in the system.
/// </summary>
public class BookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserBookRepository _userBookRepository;
    private readonly ICategoryRepository _categoryRepository;

    public BookService(
        IBookRepository bookRepository,
        IUserBookRepository userBookRepository,
        ICategoryRepository categoryRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        _userBookRepository = userBookRepository ?? throw new ArgumentNullException(nameof(userBookRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Adds a new book to the system.
    /// </summary>
    /// <param name="command">The command containing book details.</param>
    /// <returns>The created book.</returns>
    public async Task<Book> AddBookAsync(Commands.AddBookCommand command)
    {
        command.Validate();

        // Validate category if provided
        if (command.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(command.CategoryId.Value);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {command.CategoryId} not found.", nameof(command.CategoryId));
            }
        }

        var book = Book.CreateNew(
            command.Title,
            command.BookshelfId,
            command.Authors,
            command.Isbn,
            command.Publisher,
            command.PublishDate,
            command.CCode,
            command.CategoryId,
            command.CoverImageUrl,
            command.Notes);

        return await _bookRepository.AddAsync(book);
    }

    /// <summary>
    /// Updates an existing book in the system.
    /// </summary>
    /// <param name="command">The command containing updated book details.</param>
    /// <returns>The updated book, or null if the book was not found.</returns>
    public async Task<Book?> UpdateBookAsync(Commands.UpdateBookCommand command)
    {
        command.Validate();

        var existingBook = await _bookRepository.GetByIdAsync(command.Id);
        if (existingBook == null)
        {
            return null;
        }

        // Validate category if provided
        if (command.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(command.CategoryId.Value);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {command.CategoryId} not found.", nameof(command.CategoryId));
            }
        }

        var updatedBook = new Book(
            command.Id,
            command.Title,
            command.BookshelfId,
            command.Authors,
            command.Isbn,
            command.Publisher,
            command.PublishDate,
            command.CCode,
            command.CategoryId,
            command.CoverImageUrl,
            command.Notes,
            existingBook.CreatedAt,
            DateTime.UtcNow);

        return await _bookRepository.UpdateAsync(updatedBook);
    }

    /// <summary>
    /// Deletes a book from the system.
    /// </summary>
    /// <param name="command">The command containing the book ID to delete.</param>
    public async Task DeleteBookAsync(Commands.DeleteBookCommand command)
    {
        command.Validate();
        await _bookRepository.DeleteAsync(command.Id);
    }

    /// <summary>
    /// Searches for books based on specified criteria.
    /// </summary>
    /// <param name="query">The query containing search criteria.</param>
    /// <returns>An array of books matching the search criteria.</returns>
    public async Task<Book[]> SearchBooksAsync(Queries.SearchBooksQuery query)
    {
        query.Validate();

        return await _bookRepository.SearchAsync(
            query.Title,
            query.Author,
            query.Isbn?.ToString(),
            query.CategoryId,
            query.CCode?.ToString(),
            query.OwnerId,
            query.ReadingStatus);
    }

    /// <summary>
    /// Gets a book by its ID.
    /// </summary>
    /// <param name="query">The query containing the book ID.</param>
    /// <returns>The book if found; otherwise, null.</returns>
    public async Task<Book?> GetBookByIdAsync(Queries.GetBookByIdQuery query)
    {
        query.Validate();
        return await _bookRepository.GetByIdAsync(query.Id);
    }
}
