using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for adding new bookshelf users to the database.
/// </summary>
internal class AddBookshelfUserSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        INSERT INTO bookshelf_users (
            user_id,
            bookshelf_id,
            role)
        VALUES (
            @userId,
            @bookshelfId,
            @role)
        RETURNING
            user_id,
            bookshelf_id,
            role,
            created_at,
            updated_at";

    /// <summary>
    /// Executes the insert command for a new bookshelf user.
    /// </summary>
    /// <param name="bookshelfUser">The bookshelf user to insert.</param>
    /// <returns>The inserted bookshelf user as a schema.</returns>
    internal async Task<Schema.BookshelfUserSchema> ExecuteAsync(Domain.Entities.BookshelfUser bookshelfUser)
    {
        var result = await _connection.QuerySingleAsync<Schema.BookshelfUserSchema>(
            Sql,
            new
            {
                userId = bookshelfUser.UserId,
                bookshelfId = bookshelfUser.BookshelfId,
                role = bookshelfUser.Role.ToString()
            },
            _transaction);

        return result;
    }
}
