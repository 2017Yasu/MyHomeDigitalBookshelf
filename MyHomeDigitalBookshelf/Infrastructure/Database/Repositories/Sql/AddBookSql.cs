using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class AddBookSql(DbConnection connection, DbTransaction transaction) : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
INSERT INTO books (
    title, bookshelf_id, authors, isbn, publisher,
    publish_date, c_code, category_id, cover_image_url, notes
)
VALUES (
    @title, @bookshelfId, @authors,
    @isbn, @publisher, @publishDate,
    @cCode, @categoryId, @coverImageUrl, @notes
)
RETURNING id, title, bookshelf_id, authors, isbn, publisher,
          publish_date, c_code, category_id, cover_image_url,
          notes, created_at, updated_at;
";

    public async Task<BookSchema> ExecuteAsync(Book book)
    {
        return await _connection.QuerySingleAsync<BookSchema>(Sql,
            new
            {
                title = book.Title,
                bookshelfId = book.BookshelfId,
                authors = string.Join(",", book.Authors),
                isbn = book.Isbn?.ToString(),
                publisher = book.Publisher,
                publishDate = book.PublishDate,
                cCode = book.CCode?.ToString(),
                categoryId = book.CategoryId,
                coverImageUrl = book.CoverImageUrl,
                notes = book.Notes
            },
            transaction: _transaction);
    }
}
