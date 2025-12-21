using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class UpdateUserBookSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        UPDATE user_books
        SET ownership = @ownership,
            reading_status = @readingStatus,
            loan_status = @loanStatus,
            purchase_date = @purchaseDate,
            price = @price
        WHERE user_id = @userId AND book_id = @bookId
        RETURNING user_id, book_id, ownership, reading_status,
                  loan_status, purchase_date, price";

    internal async Task<Schema.UserBookSchema?> ExecuteAsync(Domain.Entities.UserBook userBook)
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

        return await _connection.QuerySingleOrDefaultAsync<Schema.UserBookSchema>(Sql, parameters, _transaction);
    }
}
