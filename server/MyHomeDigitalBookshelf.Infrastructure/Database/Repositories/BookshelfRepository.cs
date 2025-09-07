using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookshelfRepository(ILogger<BookshelfRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IBookshelfRepository
{
    public Task<Bookshelf?> GetByIdAsync(Guid id)
    {
        return QueryAndTraceAsync(
            conn => new GetBookshelvesSql(conn).QuerySingleAsync(id),
            "Get Bookshelf By Id",
            $"id: {id}");
    }

    public Task<Bookshelf[]> GetAllAsync()
    {
        return QueryAndTraceAsync(
            conn => new GetBookshelvesSql(conn).QueryAsync(),
            "Get All Bookshelves");
    }

    public Task<Bookshelf> AddAsync(Bookshelf bookshelf)
    {
        return ExecuteAndTraceAsync(
            (conn, tran) => new AddBookshelfSql(conn, tran).ExecuteAsync(bookshelf),
            "Add Bookshelf",
            bookshelf.ToString());
    }

    public Task<Bookshelf?> UpdateAsync(Bookshelf bookshelf)
    {
        return ExecuteAndTraceAsync(
            (conn, tran) => new UpdateBookshelfSql(conn, tran).ExecuteAsync(bookshelf),
            "Update Bookshelf",
            bookshelf.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>(
            (conn, tran) => new DeleteBookshelfSql(conn, tran).ExecuteAsync(id),
            "Delete Bookshelf",
            $"id: {id}");
    }
}
