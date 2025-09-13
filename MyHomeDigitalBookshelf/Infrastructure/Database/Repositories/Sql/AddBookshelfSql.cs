using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class AddBookshelfSql(DbConnection connection, DbTransaction transaction)
: ExecSqlBase(connection, transaction)
{
    private const string Sql = @"insert into
    bookshelves (name, description)
values (@name, @description)
returning
    id,
    name,
    description,
    created_at,
    updated_at";

    internal async Task<Domain.Entities.Bookshelf> ExecuteAsync(Domain.Entities.Bookshelf bookshelf)
    {
        var result = await _connection.QuerySingleAsync<Schema.BookshelfSchema>(
            Sql,
            new
            {
                name = bookshelf.Name,
                description = bookshelf.Description,
            },
            _transaction);
        return result.ToEntity();
    }
}
