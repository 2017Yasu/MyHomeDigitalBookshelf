using System;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class GetCategoryByIdSql(DbConnection connection, DbTransaction transaction)
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
WHERE id = @id";

    internal async Task<CategorySchema?> ExecuteAsync(Guid id)
    {
        return await _connection.QuerySingleOrDefaultAsync<CategorySchema>(
            Sql,
            new { id },
            _transaction);
    }
}
