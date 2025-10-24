using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for adding new sessions to the database.
/// </summary>
internal class AddSessionSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        INSERT INTO sessions (
            id,
            user_id,
            token,
            created_at,
            expires_at,
            ip_address,
            user_agent,
            refresh_token)
        VALUES (
            @id,
            @userId,
            @token,
            @createdAt,
            @expiresAt,
            @ipAddress,
            @userAgent,
            @refreshToken)
        RETURNING id, user_id, token, created_at, expires_at, ip_address, user_agent, refresh_token";

    /// <summary>
    /// Executes the insert command for a new session.
    /// </summary>
    /// <param name="session">The session to insert.</param>
    /// <returns>The inserted session as a schema.</returns>
    internal async Task<Schema.SessionSchema> ExecuteAsync(Domain.Entities.Session session)
    {
        var result = await _connection.QuerySingleAsync<Schema.SessionSchema>(
            Sql,
            new
            {
                id = session.Id,
                userId = session.UserId,
                token = session.Token,
                createdAt = session.CreatedAt,
                expiresAt = session.ExpiresAt,
                ipAddress = session.IpAddress,
                userAgent = session.UserAgent,
                refreshToken = session.RefreshToken
            },
            _transaction);

        return result;
    }
}
