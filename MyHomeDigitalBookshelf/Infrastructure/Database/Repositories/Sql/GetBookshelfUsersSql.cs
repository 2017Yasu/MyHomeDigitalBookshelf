using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for retrieving bookshelf users from the database.
/// </summary>
internal class GetBookshelfUsersSql(DbConnection connection, DbTransaction? transaction = null)
    : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"
        SELECT
            user_id,
            bookshelf_id,
            role,
            created_at,
            updated_at
        FROM bookshelf_users
        /**where**/
        ORDER BY created_at";

    /// <summary>
    /// Queries a single bookshelf user by their user ID and bookshelf ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="bookshelfId">The ID of the bookshelf.</param>
    /// <returns>A bookshelf user schema if found, null otherwise.</returns>
    internal async Task<Schema.BookshelfUserSchema?> QuerySingleAsync(Guid userId, Guid bookshelfId)
    {
        var builder = new SqlBuilder()
            .Where("user_id = @userId AND bookshelf_id = @bookshelfId", new { userId, bookshelfId });

        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Queries all bookshelf users for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>An array of bookshelf user schemas.</returns>
    internal async Task<Schema.BookshelfUserSchema[]> QueryByUserAsync(Guid userId)
    {
        var builder = new SqlBuilder()
            .Where("user_id = @userId", new { userId });
        return await QueryAsync(builder);
    }

    /// <summary>
    /// Queries all users for a given bookshelf.
    /// </summary>
    /// <param name="bookshelfId">The ID of the bookshelf.</param>
    /// <returns>An array of bookshelf user schemas.</returns>
    internal async Task<Schema.BookshelfUserSchema[]> QueryByBookshelfAsync(Guid bookshelfId)
    {
        var builder = new SqlBuilder()
            .Where("bookshelf_id = @bookshelfId", new { bookshelfId });
        return await QueryAsync(builder);
    }

    private async Task<Schema.BookshelfUserSchema[]> QueryAsync(SqlBuilder builder)
    {
        var sql = builder.AddTemplate(Sql);
        var rows = await _connection.QueryAsync<Schema.BookshelfUserSchema>(sql.RawSql, sql.Parameters, _transaction);
        return rows.ToArray();
    }
}
