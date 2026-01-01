using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

[SingletonService]
public class UserBookRepository(ILogger<UserBookRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IUserBookRepository
{
    public async Task<UserBook?> GetAsync(Guid userId, Guid bookId)
    {
        var result = await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetUserBooksSql(conn).QuerySingleAsync(userId, bookId);
                return schema?.ToEntity();
            },
            "Get UserBook",
            $"userId: {userId}, bookId: {bookId}");
        return result;
    }

    public async Task<UserBook[]> GetByUserAsync(Guid userId)
    {
        var result = await QueryAndTraceAsync<UserBook[]>(
            async conn =>
            {
                var schemas = await new GetUserBooksSql(conn).QueryByUserAsync(userId);
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Get UserBooks By User",
            $"userId: {userId}");
        return result;
    }

    public async Task<UserBook[]> GetByBookAsync(Guid bookId)
    {
        var result = await QueryAndTraceAsync<UserBook[]>(
            async conn =>
            {
                var schemas = await new GetUserBooksSql(conn).QueryByBookAsync(bookId);
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Get UserBooks By Book",
            $"bookId: {bookId}");
        return result;
    }

    public async Task<UserBook> AddAsync(UserBook userBook)
    {
        var result = await ExecuteAndTraceAsync<UserBook>(
            async (conn, tran) =>
            {
                var schema = await new AddUserBookSql(conn, tran).ExecuteAsync(userBook);
                return schema.ToEntity();
            },
            "Add UserBook",
            userBook.ToString());
        return result;
    }

    public async Task<UserBook?> UpdateAsync(UserBook userBook)
    {
        var result = await ExecuteAndTraceAsync<UserBook?>(
            async (conn, tran) =>
            {
                var schema = await new UpdateUserBookSql(conn, tran).ExecuteAsync(userBook);
                return schema?.ToEntity();
            },
            "Update UserBook",
            userBook.ToString());
        return result;
    }

    public async Task DeleteAsync(Guid userId, Guid bookId)
    {
        await ExecuteAndTraceAsync<int>(
            async (conn, tran) => await new DeleteUserBookSql(conn, tran).ExecuteAsync(userId, bookId),
            "Delete UserBook",
            $"userId: {userId}, bookId: {bookId}");
    }
}
