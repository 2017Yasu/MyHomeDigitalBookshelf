using MyHomeDigitalBookshelf.Application.UserBooks.Commands;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Application.UserBooks;

[SingletonService]
public class UserBookService
{
    private readonly IUserBookRepository _userBookRepository;

    public UserBookService(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async Task<UserBook?> UpdateUserBookStatusAsync(UpdateUserBookStatusCommand command)
    {
        command.Validate();

        var userBook = await _userBookRepository.GetAsync(command.UserId, command.BookId);

        if (userBook == null)
        {
            return null; // UserBook not found
        }

        var updatedUserBook = new UserBook(
            userBook.UserId,
            userBook.BookId,
            userBook.Ownership, // Existing ownership
            command.NewReadingStatus ?? userBook.ReadingStatus,
            command.NewLoanStatus ?? userBook.LoanStatus,
            userBook.PurchaseDate,
            userBook.Price
        );

        await _userBookRepository.UpdateAsync(updatedUserBook);
        return updatedUserBook;
    }
}
