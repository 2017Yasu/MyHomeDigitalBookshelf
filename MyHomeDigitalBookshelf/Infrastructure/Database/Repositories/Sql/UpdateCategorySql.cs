using System;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class UpdateCategorySql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
UPDATE categories
SET
    name = @name,
    description = @description,
    updated_at = CURRENT_TIMESTAMP
WHERE
    id = @id
RETURNING
    id,
    name,
    description,
    bookshelf_id,
    created_at,
    updated_at";

    internal async Task<CategorySchema?> ExecuteAsync(Category category)
    {
        return await _connection.QuerySingleOrDefaultAsync<CategorySchema>(
            Sql,
            new
            {
                id = category.Id,
                name = category.Name,
                description = category.Description
            },
            _transaction);
    }
}
