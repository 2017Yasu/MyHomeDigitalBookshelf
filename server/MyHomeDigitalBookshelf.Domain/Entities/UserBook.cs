namespace MyHomeDigitalBookshelf.Domain.Entities;

public enum ReadingStatus
{
    Unread,
    WantToRead,
    Reading,
    Completed
}

public enum LoanStatus
{
    None,
    Lent,
    Borrowed
}

public class UserBook
{
    public Guid UserId { get; }
    public Guid BookId { get; }
    public bool? Ownership { get; }
    public ReadingStatus? ReadingStatus { get; }
    public LoanStatus? LoanStatus { get; }
    public DateTime? PurchaseDate { get; }
    public ValueObjects.Price? Price { get; }
    public User? User { get; }
    public Book? Book { get; }

    public UserBook(
        Guid userId,
        Guid bookId,
        bool? ownership,
        ReadingStatus? readingStatus,
        LoanStatus? loanStatus,
        DateTime? purchaseDate,
        ValueObjects.Price? price,
        User? user = null,
        Book? book = null)
    {
        UserId = userId;
        BookId = bookId;
        Ownership = ownership;
        ReadingStatus = readingStatus;
        LoanStatus = loanStatus;
        PurchaseDate = purchaseDate;
        Price = price;
        User = user;
        Book = book;
    }
}
