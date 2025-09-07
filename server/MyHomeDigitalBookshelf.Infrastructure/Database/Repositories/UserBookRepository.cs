using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class UserBookRepository : IUserBookRepository
{
    private readonly DbConnectionProvider _connectionProvider;

    public UserBookRepository(DbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<UserBook?> GetAsync(Guid userId, Guid bookId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<UserBook>> GetByUserAsync(Guid userId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<UserBook>> GetByBookAsync(Guid bookId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task AddAsync(UserBook userBook)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(UserBook userBook)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid userId, Guid bookId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }
}
