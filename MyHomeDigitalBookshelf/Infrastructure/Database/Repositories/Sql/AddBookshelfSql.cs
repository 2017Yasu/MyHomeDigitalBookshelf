using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

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

    internal async Task<BookshelfSchema> ExecuteAsync(Bookshelf bookshelf)
    {
        return await _connection.QuerySingleAsync<BookshelfSchema>(
            Sql,
            new
            {
                name = bookshelf.Name,
                description = bookshelf.Description,
            },
            _transaction);
    }
}
