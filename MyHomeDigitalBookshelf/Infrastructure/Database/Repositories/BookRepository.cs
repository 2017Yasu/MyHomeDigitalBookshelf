using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookRepository(ILogger<BookRepository> logger, DbConnectionProvider connectionProvider)
: RepositoryBase(logger, connectionProvider), IBookRepository
{
    public async Task<Book> AddAsync(Book book)
    {
        var result = await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new AddBookSql(conn, tran).ExecuteAsync(book);
                return schema.ToEntity();
            },
            "Add New Book",
            book.ToString());
        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>(
            (conn, tran) => new DeleteBookSql(conn, tran).ExecuteAsync(id),
            "Delete Book",
            $"id: {id}");
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        var result = await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetBooksSql(conn).QuerySingleAsync(id);
                return schema?.ToEntity();
            },
            "Get Book By Id",
            $"id: {id}");
        return result;
    }

    public async Task<Book[]> SearchAsync(string? title, string? author, string? isbn, Guid? categoryId, string? cCode, Guid? ownerId, ReadingStatus? readingStatus)
    {
        var result = await QueryAndTraceAsync(
            async conn =>
            {
                var schemas = await new GetBooksSql(conn).QueryAsync(title, author, isbn, categoryId, cCode, ownerId, readingStatus);
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Search Books",
            $"title: {title}, author: {author}, isbn: {isbn}, categoryId: {categoryId}, cCode: {cCode}, ownerId: {ownerId}, readingStatus: {readingStatus}");
        return result;
    }

    public async Task<Book?> UpdateAsync(Book book)
    {
        var result = await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new UpdateBookSql(conn, tran).ExecuteAsync(book);
                return schema?.ToEntity();
            },
            "Update Book",
            book.ToString());
        return result;
    }
}
