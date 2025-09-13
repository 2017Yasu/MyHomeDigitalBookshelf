using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class UserBookRepository(ILogger<UserBookRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IUserBookRepository
{
    public Task<UserBook?> GetAsync(Guid userId, Guid bookId)
    {
        return QueryAndTraceAsync<UserBook?>(conn => throw new NotImplementedException(), "Get UserBook", $"userId: {userId}, bookId: {bookId}");
    }

    public Task<UserBook[]> GetByUserAsync(Guid userId)
    {
        return QueryAndTraceAsync<UserBook[]>(conn => throw new NotImplementedException(), "Get UserBooks By User", $"userId: {userId}");
    }

    public Task<UserBook[]> GetByBookAsync(Guid bookId)
    {
        return QueryAndTraceAsync<UserBook[]>(conn => throw new NotImplementedException(), "Get UserBooks By Book", $"bookId: {bookId}");
    }

    public Task<UserBook> AddAsync(UserBook userBook)
    {
        return ExecuteAndTraceAsync<UserBook>((conn, tran) => throw new NotImplementedException(), "Add UserBook", userBook.ToString());
    }

    public Task<UserBook?> UpdateAsync(UserBook userBook)
    {
        return ExecuteAndTraceAsync<UserBook?>((conn, tran) => throw new NotImplementedException(), "Update UserBook", userBook.ToString());
    }

    public async Task DeleteAsync(Guid userId, Guid bookId)
    {
        _ = await ExecuteAndTraceAsync<int>((conn, tran) => throw new NotImplementedException(), "Delete UserBook", $"userId: {userId}, bookId: {bookId}");
    }
}
