namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents the relationship between a user and a book, including ownership, reading, and loan status.
/// </summary>
public class UserBook
{
    /// <summary>
    /// Gets the user ID in the relationship.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the book ID in the relationship.
    /// </summary>
    public Guid BookId { get; }

    /// <summary>
    /// Gets whether the user owns the book.
    /// </summary>
    public bool? Ownership { get; }

    /// <summary>
    /// Gets the reading status of the book for the user.
    /// </summary>
    public ReadingStatus? ReadingStatus { get; }

    /// <summary>
    /// Gets the loan status of the book for the user.
    /// </summary>
    public LoanStatus? LoanStatus { get; }

    /// <summary>
    /// Gets the purchase date of the book for the user.
    /// </summary>
    public DateTime? PurchaseDate { get; }

    /// <summary>
    /// Gets the purchase price of the book for the user.
    /// </summary>
    public ValueObjects.Price? Price { get; }

    /// <summary>
    /// Gets the user entity in the relationship.
    /// </summary>
    public User? User { get; }

    /// <summary>
    /// Gets the book entity in the relationship.
    /// </summary>
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
        : this(userId, bookId, ownership, readingStatus, loanStatus, purchaseDate, price)
    {
        User = user;
        Book = book;
    }

    public static UserBook CreateNew(
        Guid userId,
        Guid bookId,
        bool? ownership,
        ReadingStatus? readingStatus,
        LoanStatus? loanStatus,
        DateTime? purchaseDate,
        ValueObjects.Price? price)
    {
        return new UserBook(userId, bookId, ownership, readingStatus, loanStatus, purchaseDate, price);
    }

    private UserBook(
        Guid userId,
        Guid bookId,
        bool? ownership,
        ReadingStatus? readingStatus,
        LoanStatus? loanStatus,
        DateTime? purchaseDate,
        ValueObjects.Price? price)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId must not be empty.", nameof(userId));
        }
        if (bookId == Guid.Empty)
        {
            throw new ArgumentException("BookId must not be empty.", nameof(bookId));
        }

        UserId = userId;
        BookId = bookId;
        Ownership = ownership;
        ReadingStatus = readingStatus;
        LoanStatus = loanStatus;
        PurchaseDate = purchaseDate;
        Price = price;
    }
    public override string ToString()
    {
        return Utilities.ClassUtilities.GetPropertiesInfo(this);
    }
}
