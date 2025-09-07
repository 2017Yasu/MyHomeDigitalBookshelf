using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class UpdateBookshelfSql(DbConnection connection, DbTransaction transaction)
: ExecSqlBase(connection, transaction)
{
    const string Sql = @"update bookshelves
set
    name = @name,
    description = @description,
    updated_at = current_timestamp
where
    id = @id
returning
    id,
    name,
    description,
    created_at,
    updated_at";

    internal async Task<Domain.Entities.Bookshelf?> ExecuteAsync(Domain.Entities.Bookshelf bookshelf)
    {
        var result = await _connection.QuerySingleOrDefaultAsync<Schema.BookshelfSchema>(
            Sql,
            new
            {
                id = bookshelf.Id,
                name = bookshelf.Name,
                description = bookshelf.Description,
            },
            _transaction);
        return result?.ToEntity();
    }
}
