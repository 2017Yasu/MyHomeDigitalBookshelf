using System;
using System.Data.Common;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using Npgsql;

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

    public async Task<Book> ExecuteAsync(Book book)
    {
        var schema = await _connection.QuerySingleAsync<Schema.BookSchema>(Sql,
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

        return schema.ToEntity();
    }
}
