using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class GetBookshelvesSql(DbConnection connection, DbTransaction? transaction = null) : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"
    SELECT
        id,
        name,
        description,
        created_at,
        updated_at
    FROM bookshelves
    /**where**/
    ORDER BY created_at";

    internal async Task<BookshelfSchema?> QuerySingleAsync(Guid id)
    {
        var builder = new SqlBuilder();
        builder.Where($@"id = @{nameof(id)}", new { id });
        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    internal async Task<BookshelfSchema[]> QueryAsync()
    {
        var builder = new SqlBuilder();
        return await QueryAsync(builder);
    }

    private async Task<BookshelfSchema[]> QueryAsync(SqlBuilder builder)
    {
        var sql = builder.AddTemplate(Sql);
        return [.. await _connection.QueryAsync<BookshelfSchema>(sql.RawSql, sql.Parameters, _transaction)];
    }
}
