using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for updating bookshelf user roles in the database.
/// </summary>
internal class UpdateBookshelfUserSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        UPDATE bookshelf_users
        SET
            role = @role,
            updated_at = CURRENT_TIMESTAMP
        WHERE user_id = @userId AND bookshelf_id = @bookshelfId
        RETURNING
            user_id,
            bookshelf_id,
            role,
            created_at,
            updated_at";

    /// <summary>
    /// Updates the role of a bookshelf user.
    /// </summary>
    /// <param name="bookshelfUser">The bookshelf user</param>
    /// <returns>The number of rows affected.</returns>
    internal async Task<BookshelfUserSchema?> ExecuteAsync(BookshelfUser bookshelfUser)
    {
        var parameters = new
        {
            userId = bookshelfUser.UserId,
            bookshelfId = bookshelfUser.BookshelfId,
            role = bookshelfUser.Role.ToString(),
        };

        return await _connection.QueryFirstOrDefaultAsync<BookshelfUserSchema>(Sql, parameters, _transaction);
    }
}
