using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.UserBooks.Commands;

public class UpdateUserBookStatusCommand
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public ReadingStatus? NewReadingStatus { get; set; }
    public LoanStatus? NewLoanStatus { get; set; }

    public void Validate()
    {
        if (UserId == Guid.Empty)
        {
            throw new ArgumentException("UserId must not be empty.", nameof(UserId));
        }

        if (BookId == Guid.Empty)
        {
            throw new ArgumentException("BookId must not be empty.", nameof(BookId));
        }
        // Basic validation for ReadingStatus enum values could be added if necessary,
        // but typically the enum itself provides type safety.
    }
}