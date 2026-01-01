using MyHomeDigitalBookshelf.Domain.Enums; // Assuming Enums are in Domain

namespace MyHomeDigitalBookshelf.Application.Books.Queries;

public class GetBooksForBookshelfQuery
{
    public Guid BookshelfId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Isbn { get; set; }
    public Guid? CategoryId { get; set; }
    public ReadingStatus? ReadingStatus { get; set; }
    public Guid? OwnerId { get; set; }

    // Pagination/Sorting
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } // e.g., "TitleAsc", "AuthorDesc"

    public void Validate()
    {
        if (BookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId must not be empty.", nameof(BookshelfId));
        }
        if (PageNumber < 1)
        {
            throw new ArgumentException("PageNumber must be at least 1.", nameof(PageNumber));
        }
        if (PageSize < 1 || PageSize > 100) // Arbitrary reasonable limit
        {
            throw new ArgumentException("PageSize must be between 1 and 100.", nameof(PageSize));
        }
    }
}
