using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

[SingletonService]
public class BookshelfUserRepository(ILogger<BookshelfUserRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IBookshelfUserRepository
{
    public Task<BookshelfUser?> GetAsync(Guid userId, Guid bookshelfId)
    {
        return QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new Sql.GetBookshelfUsersSql(conn).QuerySingleAsync(userId, bookshelfId);
                return schema?.ToEntity();
            },
            "Get BookshelfUser",
            $"userId: {userId}, bookshelfId: {bookshelfId}");
    }

    public Task<BookshelfUser[]> GetByBookshelfAsync(Guid bookshelfId)
    {
        return QueryAndTraceAsync(
            async conn =>
            {
                var schemas = await new Sql.GetBookshelfUsersSql(conn).QueryByBookshelfAsync(bookshelfId);
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Get BookshelfUsers By Bookshelf",
            $"bookshelfId: {bookshelfId}");
    }

    public Task<BookshelfUser[]> GetByUserAsync(Guid userId)
    {
        return QueryAndTraceAsync(
            async conn =>
            {
                var schemas = await new Sql.GetBookshelfUsersSql(conn).QueryByUserAsync(userId);
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Get BookshelfUsers By User",
            $"userId: {userId}");
    }

    public Task<BookshelfUser> AddAsync(BookshelfUser bookshelfUser)
    {
        return ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new Sql.AddBookshelfUserSql(conn, tran).ExecuteAsync(bookshelfUser);
                return schema.ToEntity();
            },
            "Add BookshelfUser",
            bookshelfUser.ToString());
    }

    public Task<BookshelfUser?> UpdateAsync(BookshelfUser bookshelfUser)
    {
        return ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var result = await new Sql.UpdateBookshelfUserSql(conn, tran).ExecuteAsync(bookshelfUser);
                return result?.ToEntity();
            },
            "Update BookshelfUser",
            bookshelfUser.ToString());
    }

    public async Task DeleteAsync(Guid userId, Guid bookshelfId)
    {
        await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var rowCount = await new Sql.DeleteBookshelfUserSql(conn, tran).ExecuteAsync(userId, bookshelfId);
                return rowCount;
            },
            "Delete BookshelfUser",
            $"userId: {userId}, bookshelfId: {bookshelfId}");
    }
}
