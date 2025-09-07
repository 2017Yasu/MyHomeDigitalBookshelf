using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class GetBookshelvesSql(DbConnection connection, DbTransaction? transaction = null) : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"SELECT id, name, description, created_at, updated_at FROM bookshelves /**where**/ ORDER BY created_at";

    internal async Task<Domain.Entities.Bookshelf?> QuerySingleAsync(Guid id)
    {
        var builder = new SqlBuilder();
        builder.Where($@"id = @{nameof(id)}", new { id });
        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    internal async Task<Domain.Entities.Bookshelf[]> QueryAsync()
    {
        var builder = new SqlBuilder();
        return await QueryAsync(builder);
    }

    private async Task<Domain.Entities.Bookshelf[]> QueryAsync(SqlBuilder builder)
    {
        var sql = builder.AddTemplate(Sql);
        var results = await _connection.QueryAsync<BookshelfSchema>(sql.RawSql, sql.Parameters, _transaction);
        return results.Select(r => r.ToEntity()).ToArray();
    }
}
