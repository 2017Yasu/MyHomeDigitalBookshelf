using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookshelfUserRepository(ILogger<BookshelfUserRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IBookshelfUserRepository
{
    public Task<BookshelfUser?> GetAsync(Guid userId, Guid bookshelfId)
    {
        return QueryAndTraceAsync<BookshelfUser?>(conn => throw new NotImplementedException(), "Get BookshelfUser", $"userId: {userId}, bookshelfId: {bookshelfId}");
    }

    public Task<BookshelfUser[]> GetByBookshelfAsync(Guid bookshelfId)
    {
        return QueryAndTraceAsync<BookshelfUser[]>(conn => throw new NotImplementedException(), "Get BookshelfUsers By Bookshelf", $"bookshelfId: {bookshelfId}");
    }

    public Task<BookshelfUser[]> GetByUserAsync(Guid userId)
    {
        return QueryAndTraceAsync<BookshelfUser[]>(conn => throw new NotImplementedException(), "Get BookshelfUsers By User", $"userId: {userId}");
    }

    public Task<BookshelfUser> AddAsync(BookshelfUser bookshelfUser)
    {
        return ExecuteAndTraceAsync<BookshelfUser>((conn, tran) => throw new NotImplementedException(), "Add BookshelfUser", bookshelfUser.ToString());
    }

    public Task<BookshelfUser?> UpdateAsync(BookshelfUser bookshelfUser)
    {
        return ExecuteAndTraceAsync<BookshelfUser?>((conn, tran) => throw new NotImplementedException(), "Update BookshelfUser", bookshelfUser.ToString());
    }

    public async Task DeleteAsync(Guid userId, Guid bookshelfId)
    {
        _ = await ExecuteAndTraceAsync<int>((conn, tran) => throw new NotImplementedException(), "Delete BookshelfUser", $"userId: {userId}, bookshelfId: {bookshelfId}");
    }
}
