using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class GetCategoriesSql(DbConnection connection, DbTransaction? transaction = null)
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
/**where**/
ORDER BY name";

    internal async Task<CategorySchema?> ExecuteSingleAsync(Guid id)
    {
        var builder = new SqlBuilder().Where(@$"id = @{nameof(id)}", new { id });
        var results = await ExecuteAsync(builder);
        return results.FirstOrDefault();
    }

    internal async Task<CategorySchema[]> ExecuteAsync(Guid bookshelfId)
    {
        var builder = new SqlBuilder().Where(@$"bookshelf_id = @{nameof(bookshelfId)}", new { bookshelfId });
        return await ExecuteAsync(builder);
    }

    private async Task<CategorySchema[]> ExecuteAsync()
    {
        var builder = new SqlBuilder();
        return await ExecuteAsync(builder);
    }

    private async Task<CategorySchema[]> ExecuteAsync(SqlBuilder builder)
    {
        var sql = builder.AddTemplate(Sql);
        return (await _connection.QueryAsync<CategorySchema>(sql.RawSql, sql.Parameters, _transaction)).ToArray();
    }
}
