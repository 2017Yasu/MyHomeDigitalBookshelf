using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for updating users in the database.
/// </summary>
internal class UpdateUserSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        UPDATE users
        SET
            username = @username,
            email = @email,
            password_hash = @passwordHash,
            role = @role
        WHERE id = @id
        RETURNING
            id,
            username,
            email,
            password_hash,
            role,
            created_at";

    /// <summary>
    /// Executes the update command for an existing user.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>The updated user as a schema if found; null if not found.</returns>
    internal async Task<Schema.UserSchema?> ExecuteAsync(Domain.Entities.User user)
    {
        var result = await _connection.QuerySingleOrDefaultAsync<Schema.UserSchema>(
            Sql,
            new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email?.Value,
                passwordHash = user.PasswordHash,
                role = user.Role.ToString()
            },
            _transaction);

        return result;
    }
}
