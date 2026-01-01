using MyHomeDigitalBookshelf.Domain.Enums;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.UserBooks.Commands;

public class UpdateUserBookStatusCommand
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public UserBookReadingStatus? NewReadingStatus { get; set; }
    public UserBookLoanStatus? NewLoanStatus { get; set; }

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
        if (!NewReadingStatus.HasValue && !NewLoanStatus.HasValue)
        {
            throw new ArgumentException("At least one status (Reading or Loan) must be provided for update.", nameof(NewReadingStatus));
        }
    }
}
