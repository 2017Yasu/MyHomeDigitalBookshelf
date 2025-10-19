using System;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class GetCategoriesByBookshelfSql(DbConnection connection, DbTransaction transaction)
    : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"
SELECT
    id,
    name,
    description,
    bookshelf_id,
    created_at,
    updated_at
FROM categories
WHERE bookshelf_id = @bookshelfId
ORDER BY name";

    internal async Task<CategorySchema[]> ExecuteAsync(Guid bookshelfId)
    {
        var results = await _connection.QueryAsync<CategorySchema>(
            Sql,
            new { bookshelfId },
            _transaction);
        return results.ToArray();
    }
}
