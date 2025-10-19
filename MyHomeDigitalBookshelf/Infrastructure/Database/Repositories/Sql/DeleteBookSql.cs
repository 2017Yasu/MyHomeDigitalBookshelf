using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class DeleteBookSql(DbConnection connection, DbTransaction transaction) : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"DELETE FROM books WHERE id = @id";

    public async Task<int> ExecuteAsync(Guid id)
    {
        return await _connection.ExecuteAsync(Sql,
            new { id },
            transaction: _transaction);
    }
}
