using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL queries for retrieving users from the database.
/// </summary>
internal class GetUsersSql(DbConnection connection, DbTransaction? transaction = null)
    : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"
        SELECT
            id,
            username,
            email,
            password_hash,
            role,
            created_at
        FROM users
        /**where**/
        /**orderby**/";

    /// <summary>
    /// Queries a single user by their ID.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>The user schema if found, null otherwise.</returns>
    internal async Task<Schema.UserSchema?> QuerySingleAsync(Guid id)
    {
        var builder = new Dapper.SqlBuilder()
            .Where("id = @id", new { id })
            .OrderBy("created_at DESC");

        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Queries a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The user schema if found, null otherwise.</returns>
    internal async Task<Schema.UserSchema?> QueryByUsernameAsync(string username)
    {
        var builder = new Dapper.SqlBuilder()
            .Where("username = @username", new { username })
            .OrderBy("created_at DESC");

        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Queries a user by their email.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <returns>The user schema if found, null otherwise.</returns>
    internal async Task<Schema.UserSchema?> QueryByEmailAsync(string email)
    {
        var builder = new Dapper.SqlBuilder()
            .Where("email = @email", new { email })
            .OrderBy("created_at DESC");

        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Queries all users.
    /// </summary>
    /// <returns>An array of user schemas.</returns>
    internal async Task<Schema.UserSchema[]> QueryAllAsync()
    {
        var builder = new Dapper.SqlBuilder()
            .OrderBy("created_at DESC");

        return await QueryAsync(builder);
    }

    private async Task<Schema.UserSchema[]> QueryAsync(Dapper.SqlBuilder builder)
    {
        var template = builder.AddTemplate(Sql);
        var rows = await _connection.QueryAsync<Schema.UserSchema>(
            template.RawSql,
            template.Parameters,
            _transaction);
        return rows.ToArray();
    }
}
