using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class AddCategorySql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
INSERT INTO categories
    (name, description, bookshelf_id)
VALUES
    (@name, @description, @bookshelfId)
RETURNING
    id,
    name,
    description,
    bookshelf_id,
    created_at,
    updated_at";

    internal async Task<CategorySchema> ExecuteAsync(Category category)
    {
        return await _connection.QuerySingleAsync<CategorySchema>(
            Sql,
            new
            {
                name = category.Name,
                description = category.Description,
                bookshelfId = category.BookshelfId
            },
            _transaction);
    }
}
