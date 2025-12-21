using System.Data.Common;
using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

/// <summary>
/// Repository implementation for managing user sessions in the PostgreSQL database.
/// </summary>
public class SessionRepository(ILogger<SessionRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), ISessionRepository
{
    /// <inheritdoc />
    public async Task<Session?> GetByIdAsync(Guid id)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetSessionsSql(conn).QuerySingleAsync(id);
                return schema?.ToEntity();
            },
            "Get Session By Id",
            $"id: {id}");
    }

    /// <inheritdoc />
    public async Task<Session[]> GetByUserIdAsync(Guid userId)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schemas = await new GetSessionsSql(conn).QueryByUserIdAsync(userId);
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Get Sessions By UserId",
            $"userId: {userId}");
    }

    /// <inheritdoc />
    public async Task<Session> AddAsync(Session session)
    {
        return await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new AddSessionSql(conn, tran).ExecuteAsync(session);
                return schema.ToEntity();
            },
            "Add Session",
            session.ToString());
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync(
            async (conn, tran) => await new DeleteSessionSql(conn, tran).ExecuteAsync(id),
            "Delete Session",
            $"id: {id}");
    }

    /// <summary>
    /// Deletes all expired sessions from the database.
    /// </summary>
    /// <returns>The number of sessions deleted.</returns>
    public async Task<int> DeleteExpiredAsync()
    {
        return await ExecuteAndTraceAsync(
            async (conn, tran) => await new DeleteSessionSql(conn, tran).DeleteExpiredAsync(),
            "Delete Expired Sessions");
    }
}
