using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class AddUserBookSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        INSERT INTO user_books (user_id, book_id, ownership, reading_status,
                              loan_status, purchase_date, price)
        VALUES (@userId, @bookId, @ownership, @readingStatus,
                @loanStatus, @purchaseDate, @price)
        RETURNING user_id, book_id, ownership, reading_status,
                  loan_status, purchase_date, price";

    internal async Task<Schema.UserBookSchema> ExecuteAsync(Domain.Entities.UserBook userBook)
    {
        var parameters = new
        {
            userId = userBook.UserId,
            bookId = userBook.BookId,
            ownership = userBook.Ownership,
            readingStatus = userBook.ReadingStatus?.ToString(),
            loanStatus = userBook.LoanStatus?.ToString(),
            purchaseDate = userBook.PurchaseDate,
            price = userBook.Price?.Value
        };

        return await _connection.QuerySingleAsync<Schema.UserBookSchema>(Sql, parameters, _transaction);
    }
}
