using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbConnectionProvider _connectionProvider;

    public CategoryRepository(DbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Category>> GetAllByBookshelfAsync(Guid bookshelfId)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task AddAsync(Category category)
    {
        await using var conn = await _connectionProvider.GetConnection();
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Category category)
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
