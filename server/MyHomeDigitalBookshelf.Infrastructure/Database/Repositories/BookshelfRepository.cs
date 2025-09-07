using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookshelfRepository : IBookshelfRepository
{
    private readonly DbConnectionProvider _connectionProvider;

    public BookshelfRepository(DbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<Bookshelf?> GetByIdAsync(Guid id)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Bookshelf>> GetAllAsync()
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task AddAsync(Bookshelf bookshelf)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Bookshelf bookshelf)
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
