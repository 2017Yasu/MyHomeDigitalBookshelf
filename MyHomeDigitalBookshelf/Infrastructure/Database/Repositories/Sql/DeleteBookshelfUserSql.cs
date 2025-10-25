using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for removing users from bookshelves in the database.
/// </summary>
internal class DeleteBookshelfUserSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        DELETE FROM bookshelf_users
        WHERE user_id = @userId AND bookshelf_id = @bookshelfId";

    /// <summary>
    /// Removes a user from a bookshelf.
    /// </summary>
    /// <param name="userId">The ID of the user to remove.</param>
    /// <param name="bookshelfId">The ID of the bookshelf.</param>
    /// <returns>The number of rows affected.</returns>
    internal async Task<int> ExecuteAsync(Guid userId, Guid bookshelfId)
    {
        var parameters = new { userId, bookshelfId };
        return await _connection.ExecuteAsync(Sql, parameters, _transaction);
    }
}
