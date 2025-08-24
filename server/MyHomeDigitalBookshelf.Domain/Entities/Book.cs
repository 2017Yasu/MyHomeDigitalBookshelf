namespace MyHomeDigitalBookshelf.Domain.Entities;

public class Book
{
    public Guid Id { get; }
    public string Title { get; }
    public string[] Authors { get; }
    public ValueObjects.Isbn? Isbn { get; }
    public string? Publisher { get; }
    public DateTime? PublishDate { get; }
    public ValueObjects.CCode? CCode { get; }
    public Guid? CategoryId { get; }
    public string? CoverImageUrl { get; }
    public string? Notes { get; }
    public Guid BookshelfId { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public Category? Category { get; }
    public Bookshelf? Bookshelf { get; }
    public UserBook[] UserBooks { get; }

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
        Category? category = null,
        Bookshelf? bookshelf = null,
        UserBook[]? userBooks = null)
    {
        Id = id;
        Title = title;
        Authors = authors ?? Array.Empty<string>();
        Isbn = isbn;
        Publisher = publisher;
        PublishDate = publishDate;
        CCode = cCode;
        CategoryId = categoryId;
        CoverImageUrl = coverImageUrl;
        Notes = notes;
        BookshelfId = bookshelfId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Category = category;
        Bookshelf = bookshelf;
        UserBooks = userBooks ?? Array.Empty<UserBook>();
    }
}
