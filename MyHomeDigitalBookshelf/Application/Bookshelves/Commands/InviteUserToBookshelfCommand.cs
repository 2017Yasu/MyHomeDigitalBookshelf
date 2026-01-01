namespace MyHomeDigitalBookshelf.Application.Bookshelves.Commands;

public class InviteUserToBookshelfCommand
{
    public Guid BookshelfId { get; set; }
    public string InvitedUserEmail { get; set; } = string.Empty;
    public Guid InvitingUserId { get; set; }

    public void Validate()
    {
        if (BookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId must not be empty.", nameof(BookshelfId));
        }
        if (string.IsNullOrWhiteSpace(InvitedUserEmail))
        {
            throw new ArgumentException("InvitedUserEmail must not be null or whitespace.", nameof(InvitedUserEmail));
        }
        if (InvitingUserId == Guid.Empty)
        {
            throw new ArgumentException("InvitingUserId must not be empty.", nameof(InvitingUserId));
        }
    }
}
