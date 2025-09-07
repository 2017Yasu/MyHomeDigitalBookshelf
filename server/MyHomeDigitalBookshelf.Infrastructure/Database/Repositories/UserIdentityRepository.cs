using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class UserIdentityRepository : IUserIdentityRepository
{
    private readonly DbConnectionProvider _connectionProvider;

    public UserIdentityRepository(DbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<UserIdentity?> GetByIdAsync(Guid id)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task<UserIdentity?> GetByProviderAndSubjectAsync(string provider, string subject)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task AddAsync(UserIdentity identity)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }
}
