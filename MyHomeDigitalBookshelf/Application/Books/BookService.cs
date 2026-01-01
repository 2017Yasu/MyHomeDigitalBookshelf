using MyHomeDigitalBookshelf.Application.Books.Interfaces;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.Books;

/// <summary>
/// Service for managing books in the system.
/// </summary>
public class BookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserBookRepository _userBookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBookFinderService _bookFinderService;

    public BookService(
        IBookRepository bookRepository,
        IUserBookRepository userBookRepository,
        ICategoryRepository categoryRepository,
        IBookFinderService bookFinderService)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        _userBookRepository = userBookRepository ?? throw new ArgumentNullException(nameof(userBookRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _bookFinderService = bookFinderService ?? throw new ArgumentNullException(nameof(bookFinderService));
    }

    /// <summary>
    /// Adds a new book to the system by fetching details from an external service using ISBN.
    /// </summary>
    /// <param name="command">The command containing the ISBN and bookshelf details.</param>
    /// <returns>The created book.</returns>
    public async Task<Book> AddBookFromIsbnAsync(Commands.AddBookFromIsbnCommand command)
    {
        command.Validate();

        var existingBook = await _bookRepository.GetByIsbnAsync(new Isbn(command.Isbn));
        if (existingBook != null)
        {
            throw new InvalidOperationException($"Book with ISBN {command.Isbn} already exists.");
        }

        var foundBook = await _bookFinderService.FindByIsbnAsync(command.Isbn);
        if (foundBook == null)
        {
            throw new ArgumentException($"Book with ISBN {command.Isbn} not found by external service.");
        }

        // We need to create a new Book instance with the correct BookshelfId and potentially other owner-specific info
        // as the one returned by FindByIsbnAsync might not have these context-specific details.
        var bookToAdd = Book.CreateNew(
            foundBook.Title,
            command.BookshelfId, // Use the bookshelfId from the command
            foundBook.Authors,
            foundBook.Isbn,
            foundBook.Publisher,
            foundBook.PublishDate,
            foundBook.CCode,
            foundBook.CategoryId, // Potentially found from external service or null
            foundBook.CoverImageUrl,
            foundBook.Notes
        );

        var addedBook = await _bookRepository.AddAsync(bookToAdd); // Add book first to get assigned Id

        // Now create a UserBook entry using the assigned Id from addedBook
        var userBook = UserBook.CreateNew(
            command.OwnerId,
            addedBook.Id, // Use the assigned Id from addedBook
            true, // Assuming the user owns the book if they add it
            ReadingStatus.WantToRead,
            LoanStatus.None,
            null, // No purchase date on ISBN add
            null // No price on ISBN add
        );

        await _userBookRepository.AddAsync(userBook); // Add the user-book association

        return addedBook;
    } // Correct closing brace for AddBookFromIsbnAsync method

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
            title: query.Title,
            author: query.Author,
            isbn: query.Isbn?.ToString(),
            categoryId: query.CategoryId,
            cCode: query.CCode?.ToString(),
            ownerId: query.OwnerId,
            bookshelfId: null, // This is a general search, not tied to a specific bookshelf
            readingStatus: query.ReadingStatus);
    }

    /// <summary>
    /// Gets books for a specific bookshelf, with optional filtering and sorting.
    /// </summary>
    /// <param name="query">The query containing bookshelf ID, filters, and sorting/pagination info.</param>
    /// <returns>An array of books matching the criteria.</returns>
    public async Task<Book[]> GetBooksForBookshelfAsync(Queries.GetBooksForBookshelfQuery query)
    {
        query.Validate();

        // Delegate the complex filtering, sorting, and pagination to the repository.
        // The repository should handle mapping ReadingStatus enum to its string representation if needed.
        return await _bookRepository.SearchAsync(
            title: query.Title,
            author: query.Author,
            isbn: query.Isbn,
            categoryId: query.CategoryId,
            cCode: null, // Assuming CCode not part of this specific query in GetBooksForBookshelfQuery
            ownerId: query.OwnerId,
            bookshelfId: query.BookshelfId, // Ensure repository filters by bookshelf
            readingStatus: query.ReadingStatus);
        // TODO: Add support for sorting and pagination in IBookRepository.SearchAsync
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
