using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class CategoryRepository(ILogger<CategoryRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), ICategoryRepository
{
    public Task<Category?> GetByIdAsync(Guid id)
    {
        return QueryAndTraceAsync<Category?>(conn => throw new NotImplementedException(), "Get Category By Id", $"id: {id}");
    }

    public Task<Category[]> GetAllByBookshelfAsync(Guid bookshelfId)
    {
        return QueryAndTraceAsync<Category[]>(conn => throw new NotImplementedException(), "Get Categories By Bookshelf", $"bookshelfId: {bookshelfId}");
    }

    public Task<Category> AddAsync(Category category)
    {
        return ExecuteAndTraceAsync<Category>((conn, tran) => throw new NotImplementedException(), "Add Category", category.ToString());
    }

    public Task<Category?> UpdateAsync(Category category)
    {
        return ExecuteAndTraceAsync<Category?>((conn, tran) => throw new NotImplementedException(), "Update Category", category.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>((conn, tran) => throw new NotImplementedException(), "Delete Category", $"id: {id}");
    }
}
