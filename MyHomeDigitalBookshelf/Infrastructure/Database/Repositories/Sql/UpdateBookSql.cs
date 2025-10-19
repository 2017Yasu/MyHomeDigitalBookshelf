using System.Data.Common;
using System.Text.Json;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class UpdateBookSql(DbConnection connection, DbTransaction transaction) : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        UPDATE books SET
            title = @title,
            bookshelf_id = @bookshelfId,
            authors = @authors,
            isbn = @isbn,
            publisher = @publisher,
            publish_date = @publishDate,
            c_code = @cCode,
            category_id = @categoryId,
            cover_image_url = @coverImageUrl,
            notes = @notes,
            updated_at = CURRENT_TIMESTAMP
        WHERE id = @id
        RETURNING id, title, bookshelf_id, authors, isbn, publisher,
                  publish_date, c_code, category_id, cover_image_url,
                  notes, created_at, updated_at";

    public async Task<Book?> ExecuteAsync(Book book)
    {
        var schema = await _connection.QuerySingleOrDefaultAsync<Schema.BookSchema>(Sql,
            new
            {
                id = book.Id,
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

        return schema?.ToEntity();
    }
}
