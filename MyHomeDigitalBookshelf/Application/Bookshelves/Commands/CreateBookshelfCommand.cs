namespace MyHomeDigitalBookshelf.Application.Bookshelves.Commands;

public class CreateBookshelfCommand
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Name must not be null or whitespace.", nameof(Name));
        }
        if (OwnerId == Guid.Empty)
        {
            throw new ArgumentException("OwnerId must not be empty.", nameof(OwnerId));
        }
    }
}
