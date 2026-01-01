namespace MyHomeDigitalBookshelf.Application.Bookshelves.Queries;

public class GetBookshelfByIdQuery
{
    public Guid Id { get; set; }

    public void Validate()
    {
        if (Id == Guid.Empty)
        {
            throw new ArgumentException("Id must not be empty.", nameof(Id));
        }
    }
}