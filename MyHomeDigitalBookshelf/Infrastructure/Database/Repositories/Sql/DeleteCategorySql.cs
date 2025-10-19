using System;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class DeleteCategorySql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
DELETE FROM categories
WHERE id = @id";

    internal async Task<int> ExecuteAsync(Guid id)
    {
        return await _connection.ExecuteAsync(
            Sql,
            new { id },
            _transaction);
    }
}
