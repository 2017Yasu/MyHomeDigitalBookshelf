using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

public class UserBookSchema
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public bool? Ownership { get; set; }
    public string? ReadingStatus { get; set; }
    public string? LoanStatus { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? Price { get; set; }

    public UserSchema? User { get; set; }
    public BookSchema? Book { get; set; }

    public UserBook ToEntity()
    {
        return new(
            userId: UserId,
            bookId: BookId,
            ownership: Ownership,
            readingStatus: !string.IsNullOrEmpty(ReadingStatus) ?
                Enum.Parse<Domain.Entities.ReadingStatus>(ReadingStatus) : null,
            loanStatus: !string.IsNullOrEmpty(LoanStatus) ?
                Enum.Parse<Domain.Entities.LoanStatus>(LoanStatus) : null,
            purchaseDate: PurchaseDate,
            price: Price.HasValue ? new Domain.ValueObjects.Price(Price.Value) : null,
            user: User?.ToEntity(),
            book: Book?.ToEntity());
    }
}
