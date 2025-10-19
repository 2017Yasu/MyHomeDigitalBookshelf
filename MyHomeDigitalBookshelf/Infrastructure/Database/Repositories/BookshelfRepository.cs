using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookshelfRepository(ILogger<BookshelfRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IBookshelfRepository
{
    public async Task<Bookshelf?> GetByIdAsync(Guid id)
    {
        var result = await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetBookshelvesSql(conn).QuerySingleAsync(id);
                return schema?.ToEntity();
            },
            "Get Bookshelf By Id",
            $"id: {id}");
        return result;
    }

    public async Task<Bookshelf[]> GetAllAsync()
    {
        var result = await QueryAndTraceAsync(
            async conn =>
            {
                var schemas = await new GetBookshelvesSql(conn).QueryAsync();
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Get All Bookshelves");
        return result;
    }

    public async Task<Bookshelf> AddAsync(Bookshelf bookshelf)
    {
        var result = await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new AddBookshelfSql(conn, tran).ExecuteAsync(bookshelf);
                return schema.ToEntity();
            },
            "Add Bookshelf",
            bookshelf.ToString());
        return result;
    }

    public async Task<Bookshelf?> UpdateAsync(Bookshelf bookshelf)
    {
        var result = await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new UpdateBookshelfSql(conn, tran).ExecuteAsync(bookshelf);
                return schema?.ToEntity();
            },
            "Update Bookshelf",
            bookshelf.ToString());
        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>(
            (conn, tran) => new DeleteBookshelfSql(conn, tran).ExecuteAsync(id),
            "Delete Bookshelf",
            $"id: {id}");
    }
}
