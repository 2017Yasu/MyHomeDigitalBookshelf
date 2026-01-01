using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.UserBooks.Commands;

public class UpdateUserBookStatusCommand
{
    public Guid UserBookId { get; set; }
    public ReadingStatus NewReadingStatus { get; set; }

    public void Validate()
    {
        if (UserBookId == Guid.Empty)
        {
            throw new ArgumentException("UserBookId must not be empty.", nameof(UserBookId));
        }
        // Basic validation for ReadingStatus enum values could be added if necessary,
        // but typically the enum itself provides type safety.
    }
}