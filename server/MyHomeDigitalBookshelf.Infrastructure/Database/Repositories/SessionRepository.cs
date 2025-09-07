using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class SessionRepository(ILogger<SessionRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), ISessionRepository
{
    public Task<Session?> GetByIdAsync(Guid id)
    {
        return QueryAndTraceAsync<Session?>(conn => throw new NotImplementedException(), "Get Session By Id", $"id: {id}");
    }

    public Task<Session[]> GetByUserIdAsync(Guid userId)
    {
        return QueryAndTraceAsync<Session[]>(conn => throw new NotImplementedException(), "Get Sessions By UserId", $"userId: {userId}");
    }

    public Task<Session> AddAsync(Session session)
    {
        return ExecuteAndTraceAsync<Session>((conn, tran) => throw new NotImplementedException(), "Add Session", session.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>((conn, tran) => throw new NotImplementedException(), "Delete Session", $"id: {id}");
    }
}
