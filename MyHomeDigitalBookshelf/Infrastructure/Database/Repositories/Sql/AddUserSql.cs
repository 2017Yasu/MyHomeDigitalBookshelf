using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for adding new users to the database.
/// </summary>
internal class AddUserSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        INSERT INTO users (
            username,
            email,
            password_hash,
            role)
        VALUES (
            @username,
            @email,
            @passwordHash,
            @role)
        RETURNING
            id,
            username,
            email,
            password_hash,
            role,
            created_at";

    /// <summary>
    /// Executes the insert command for a new user.
    /// </summary>
    /// <param name="user">The user to insert.</param>
    /// <returns>The inserted user as a schema.</returns>
    internal async Task<Schema.UserSchema> ExecuteAsync(Domain.Entities.User user)
    {
        var result = await _connection.QuerySingleAsync<Schema.UserSchema>(
            Sql,
            new
            {
                username = user.Username,
                email = user.Email?.Value,
                passwordHash = user.PasswordHash,
                role = user.Role.ToString()
            },
            _transaction);

        return result;
    }
}
