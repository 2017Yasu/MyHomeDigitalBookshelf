using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class DeleteBookshelfSql(DbConnection connection, DbTransaction transaction)
: ExecSqlBase(connection, transaction)
{
    const string Sql = @"delete from
  bookshelves
where
  id = @id";

    internal async Task<int> ExecuteAsync(Guid bookshelfId)
    {
        return await _connection.ExecuteAsync(
            Sql,
            new { id = bookshelfId },
            _transaction);
    }
}
