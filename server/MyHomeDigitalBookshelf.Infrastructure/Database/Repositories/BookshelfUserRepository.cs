using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookshelfUserRepository : IBookshelfUserRepository
{
    private readonly DbConnectionProvider _connectionProvider;

    public BookshelfUserRepository(DbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<BookshelfUser?> GetAsync(Guid userId, Guid bookshelfId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BookshelfUser>> GetByBookshelfAsync(Guid bookshelfId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BookshelfUser>> GetByUserAsync(Guid userId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task AddAsync(BookshelfUser bookshelfUser)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(BookshelfUser bookshelfUser)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid userId, Guid bookshelfId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }
}
