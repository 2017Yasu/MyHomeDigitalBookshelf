namespace MyHomeDigitalBookshelf.Application.Books.Commands;

public class AddBookFromIsbnCommand
{
    public string Isbn { get; set; } = string.Empty;
    public Guid BookshelfId { get; set; }
    public Guid OwnerId { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Isbn))
        {
            throw new ArgumentException("ISBN must not be null or whitespace.", nameof(Isbn));
        }
        if (BookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId must not be empty.", nameof(BookshelfId));
        }
        if (OwnerId == Guid.Empty)
        {
            throw new ArgumentException("OwnerId must not be empty.", nameof(OwnerId));
        }
    }
}
