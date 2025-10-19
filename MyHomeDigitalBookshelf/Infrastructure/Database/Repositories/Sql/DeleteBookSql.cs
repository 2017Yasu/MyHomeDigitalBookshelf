using System;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class DeleteBookSql : ExecSqlBase
{
    public DeleteBookSql(DbConnection connection, DbTransaction transaction)
        : base(connection, transaction)
    {
    }

    private const string Sql = @"DELETE FROM books WHERE id = @id";

    public async Task<int> ExecuteAsync(Guid id)
    {
        return await _connection.ExecuteAsync(Sql,
            new { id },
            transaction: _transaction);
    }
}
