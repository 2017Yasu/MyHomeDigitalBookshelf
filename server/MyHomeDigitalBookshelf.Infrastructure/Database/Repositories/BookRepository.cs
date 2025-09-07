using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class BookRepository(ILogger<BookRepository> logger, DbConnectionProvider connectionProvider)
: RepositoryBase(logger, connectionProvider), IBookRepository
{
    public Task<Book> AddAsync(Book book)
    {
        return ExecuteAndTraceAsync<Book>((conn, tran) => throw new NotImplementedException(), "Add New Book", book.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>((conn, tran) => throw new NotImplementedException(), "Delete Book", $"id: {id}");
    }

    public Task<Book?> GetByIdAsync(Guid id)
    {
        return QueryAndTraceAsync<Book?>(conn => throw new NotImplementedException(), "Get Book By Id", $"id: {id}");
    }

    public Task<Book[]> SearchAsync(string? title, string? author, string? isbn, Guid? categoryId, string? cCode, Guid? ownerId, ReadingStatus? readingStatus)
    {
        return QueryAndTraceAsync<Book[]>(conn => throw new NotImplementedException(), "Search Books", $"title: {title}, author: {author}, isbn: {isbn}, categoryId: {categoryId}, cCode: {cCode}, ownerId: {ownerId}, readingStatus: {readingStatus}");
    }

    public Task<Book?> UpdateAsync(Book book)
    {
        return ExecuteAndTraceAsync<Book?>((conn, tran) => throw new NotImplementedException(), "Update Book", book.ToString());
    }
}
