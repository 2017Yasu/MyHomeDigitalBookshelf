using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookshelfRepository(ILogger<BookshelfRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IBookshelfRepository
{
    public Task<Bookshelf?> GetByIdAsync(Guid id)
    {
        return QueryAndTraceAsync<Bookshelf?>(conn => throw new NotImplementedException(), "Get Bookshelf By Id", $"id: {id}");
    }

    public Task<Bookshelf[]> GetAllAsync()
    {
        return QueryAndTraceAsync<Bookshelf[]>(conn => throw new NotImplementedException(), "Get All Bookshelves");
    }

    public Task<Bookshelf> AddAsync(Bookshelf bookshelf)
    {
        return ExecuteAndTraceAsync<Bookshelf>((conn, tran) => throw new NotImplementedException(), "Add Bookshelf", bookshelf.ToString());
    }

    public Task<Bookshelf?> UpdateAsync(Bookshelf bookshelf)
    {
        return ExecuteAndTraceAsync<Bookshelf?>((conn, tran) => throw new NotImplementedException(), "Update Bookshelf", bookshelf.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>((conn, tran) => throw new NotImplementedException(), "Delete Bookshelf", $"id: {id}");
    }
}
