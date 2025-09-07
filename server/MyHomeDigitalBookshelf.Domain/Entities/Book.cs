namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents a book in the digital bookshelf, including metadata and categorization.
/// </summary>
public class Book
{
    /// <summary>
    /// Gets the unique identifier for the book.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the title of the book.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the list of authors for the book.
    /// </summary>
    public string[] Authors { get; }

    /// <summary>
    /// Gets the ISBN value object representing the book's ISBN.
    /// </summary>
    public ValueObjects.Isbn? Isbn { get; }

    /// <summary>
    /// Gets the publisher of the book.
    /// </summary>
    public string? Publisher { get; }

    /// <summary>
    /// Gets the publication date of the book.
    /// </summary>
    public DateTime? PublishDate { get; }

    /// <summary>
    /// Gets the Japanese C-Code classification for the book.
    /// </summary>
    public ValueObjects.CCode? CCode { get; }

    /// <summary>
    /// Gets the category ID to which the book belongs.
    /// </summary>
    public Guid? CategoryId { get; }

    /// <summary>
    /// Gets the URL of the book's cover image.
    /// </summary>
    public string? CoverImageUrl { get; }

    /// <summary>
    /// Gets any notes or comments about the book.
    /// </summary>
    public string? Notes { get; }

    /// <summary>
    /// Gets the ID of the bookshelf to which the book belongs.
    /// </summary>
    public Guid BookshelfId { get; }

    /// <summary>
    /// Gets the timestamp when the book was created in the system.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when the book was last updated in the system.
    /// </summary>
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Gets the category entity to which the book belongs.
    /// </summary>
    public Category? Category { get; }

    public Book(
        Guid id,
        string title,
        string[]? authors = null,
        ValueObjects.Isbn? isbn = null,
        string? publisher = null,
        DateTime? publishDate = null,
        ValueObjects.CCode? cCode = null,
        Guid? categoryId = null,
        string? coverImageUrl = null,
        string? notes = null,
        Guid bookshelfId = default,
        DateTime createdAt = default,
        DateTime updatedAt = default,
        Category? category = null)
        : this(title, authors, isbn, publisher, publishDate, cCode, categoryId, coverImageUrl, notes, bookshelfId)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id must not be empty.", nameof(id));
        }
        if (createdAt == default)
        {
            throw new ArgumentException("CreatedAt must be a valid date.", nameof(createdAt));
        }
        if (updatedAt == default)
        {
            throw new ArgumentException("UpdatedAt must be a valid date.", nameof(updatedAt));
        }

        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Category = category;
    }

    public static Book CreateNew(
        string title,
        string[]? authors = null,
        ValueObjects.Isbn? isbn = null,
        string? publisher = null,
        DateTime? publishDate = null,
        ValueObjects.CCode? cCode = null,
        Guid? categoryId = null,
        string? coverImageUrl = null,
        string? notes = null,
        Guid bookshelfId = default)
    {
        return new Book(title, authors, isbn, publisher, publishDate, cCode, categoryId, coverImageUrl, notes, bookshelfId);
    }

    private Book(
        string title,
        string[]? authors,
        ValueObjects.Isbn? isbn,
        string? publisher,
        DateTime? publishDate,
        ValueObjects.CCode? cCode,
        Guid? categoryId,
        string? coverImageUrl,
        string? notes,
        Guid bookshelfId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must not be null or whitespace.", nameof(title));
        }
        if (bookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId must not be empty.", nameof(bookshelfId));
        }
        if (authors != null && authors.Any(a => string.IsNullOrWhiteSpace(a)))
        {
            throw new ArgumentException("Authors array must not contain null or whitespace elements.", nameof(authors));
        }

        Title = title;
        Authors = authors ?? [];
        Isbn = isbn;
        Publisher = publisher;
        PublishDate = publishDate;
        CCode = cCode;
        CategoryId = categoryId;
        CoverImageUrl = coverImageUrl;
        Notes = notes;
        BookshelfId = bookshelfId;
    }
}
