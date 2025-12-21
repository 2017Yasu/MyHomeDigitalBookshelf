using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class DeleteUserBookSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        DELETE FROM user_books
        WHERE user_id = @userId AND book_id = @bookId";

    internal async Task<int> ExecuteAsync(Guid userId, Guid bookId)
    {
        return await _connection.ExecuteAsync(Sql, new { userId, bookId }, _transaction);
    }
}
