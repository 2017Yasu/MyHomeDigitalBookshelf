using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL queries for retrieving sessions from the database.
/// </summary>
internal class GetSessionsSql(DbConnection connection, DbTransaction? transaction = null)
    : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"
        SELECT
            id,
            user_id,
            token,
            created_at,
            expires_at,
            ip_address,
            user_agent,
            refresh_token
        FROM sessions
        /**where**/
        ORDER BY created_at DESC";

    /// <summary>
    /// Queries a single session by its ID.
    /// </summary>
    /// <param name="id">The session ID to search for.</param>
    /// <returns>The session schema if found, null otherwise.</returns>
    internal async Task<Schema.SessionSchema?> QuerySingleAsync(Guid id)
    {
        var builder = new SqlBuilder()
            .Where("id = @id", new { id });

        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Queries a session by its token.
    /// </summary>
    /// <param name="token">The token to search for.</param>
    /// <returns>The session schema if found, null otherwise.</returns>
    internal async Task<Schema.SessionSchema?> QueryByTokenAsync(string token)
    {
        var builder = new SqlBuilder()
            .Where("token = @token", new { token });

        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Queries sessions for a specific user.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <returns>An array of session schemas.</returns>
    internal async Task<Schema.SessionSchema[]> QueryByUserIdAsync(Guid userId)
    {
        var builder = new SqlBuilder()
            .Where("user_id = @userId", new { userId });

        return await QueryAsync(builder);
    }

    /// <summary>
    /// Queries all expired sessions.
    /// </summary>
    /// <returns>An array of session schemas.</returns>
    internal async Task<Schema.SessionSchema[]> QueryExpiredAsync()
    {
        var builder = new SqlBuilder()
            .Where("expires_at < CURRENT_TIMESTAMP");

        return await QueryAsync(builder);
    }

    private async Task<Schema.SessionSchema[]> QueryAsync(SqlBuilder builder)
    {
        var template = builder.AddTemplate(Sql);
        var rows = await _connection.QueryAsync<Schema.SessionSchema>(
            template.RawSql,
            template.Parameters,
            _transaction);
        return rows.ToArray();
    }
}
