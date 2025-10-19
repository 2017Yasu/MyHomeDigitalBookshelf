using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookRepository(ILogger<BookRepository> logger, DbConnectionProvider connectionProvider)
: RepositoryBase(logger, connectionProvider), IBookRepository
{
    public Task<Book> AddAsync(Book book)
    {
        return ExecuteAndTraceAsync(
            (conn, tran) => new AddBookSql(conn, tran).ExecuteAsync(book),
            "Add New Book",
            book.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>(
            (conn, tran) => new DeleteBookSql(conn, tran).ExecuteAsync(id),
            "Delete Book",
            $"id: {id}");
    }

    public Task<Book?> GetByIdAsync(Guid id)
    {
        return QueryAndTraceAsync(
            conn => new GetBooksSql(conn).QuerySingleAsync(id),
            "Get Book By Id",
            $"id: {id}");
    }

    public Task<Book[]> SearchAsync(string? title, string? author, string? isbn, Guid? categoryId, string? cCode, Guid? ownerId, ReadingStatus? readingStatus)
    {
        return QueryAndTraceAsync(
            conn => new GetBooksSql(conn).QueryAsync(title, author, isbn, categoryId, cCode, ownerId, readingStatus),
            "Search Books",
            $"title: {title}, author: {author}, isbn: {isbn}, categoryId: {categoryId}, cCode: {cCode}, ownerId: {ownerId}, readingStatus: {readingStatus}");
    }

    public Task<Book?> UpdateAsync(Book book)
    {
        return ExecuteAndTraceAsync(
            (conn, tran) => new UpdateBookSql(conn, tran).ExecuteAsync(book),
            "Update Book",
            book.ToString());
    }
}
