using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class UserIdentityRepository(ILogger<UserIdentityRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IUserIdentityRepository
{
    public Task<UserIdentity?> GetByIdAsync(Guid id)
    {
        return QueryAndTraceAsync<UserIdentity?>(conn => throw new NotImplementedException(), "Get UserIdentity By Id", $"id: {id}");
    }

    public Task<UserIdentity?> GetByProviderAndSubjectAsync(string provider, string subject)
    {
        return QueryAndTraceAsync<UserIdentity?>(conn => throw new NotImplementedException(), "Get UserIdentity By Provider/Subject", $"provider: {provider}, subject: {subject}");
    }

    public Task<UserIdentity> AddAsync(UserIdentity identity)
    {
        return ExecuteAndTraceAsync<UserIdentity>((conn, tran) => throw new NotImplementedException(), "Add UserIdentity", identity.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>((conn, tran) => throw new NotImplementedException(), "Delete UserIdentity", $"id: {id}");
    }
}
