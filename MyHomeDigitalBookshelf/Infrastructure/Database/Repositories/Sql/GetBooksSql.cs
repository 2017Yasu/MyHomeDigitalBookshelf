using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.ValueObjects; // Added for Isbn
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class GetBooksSql(DbConnection connection, DbTransaction? transaction = null) : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"
        SELECT
            b.id,
            b.title,
            b.bookshelf_id,
            b.authors,
            b.isbn,
            b.publisher,
            b.publish_date,
            b.c_code,
            b.category_id,
            b.cover_image_url,
            b.notes,
            b.created_at,
            b.updated_at,
            c.id AS cat_id,
            c.name AS cat_name,
            c.description AS cat_description,
            c.bookshelf_id AS cat_bookshelf_id,
            c.created_at AS cat_created_at,
            c.updated_at AS cat_updated_at
        FROM books b
            LEFT JOIN categories c ON b.category_id = c.id
        /**leftjoin**/
        /**where**/";

    public async Task<BookSchema?> QuerySingleAsync(Guid id)
    {
        var builder = new SqlBuilder().Where(@$"b.id = @{nameof(id)}", new { id });
        return (await QueryAsync(builder)).FirstOrDefault();
    }

    public async Task<BookSchema?> QueryByIsbnAsync(string isbn)
    {
        var builder = new SqlBuilder().Where(@$"b.isbn = @{nameof(isbn)}", new { isbn });
        return (await QueryAsync(builder)).FirstOrDefault();
    }

    public async Task<BookSchema[]> QueryAsync(
        string? title, string? author, string? isbn, Guid? categoryId,
        string? cCode, Guid? ownerId, Guid? bookshelfId, ReadingStatus? readingStatus) // Modified signature
    {
        var builder = new SqlBuilder().LeftJoin("user_books ub ON b.id = ub.book_id");
        if (!string.IsNullOrEmpty(title))
        {
            builder = builder.Where(@$"b.title LIKE '%' || @{nameof(title)} || '%'", new { title });
        }
        if (!string.IsNullOrEmpty(author))
        {
            builder = builder.Where(@$"b.authors LIKE '%' || @{nameof(author)} || '%'", new { author });
        }
        if (!string.IsNullOrEmpty(isbn))
        {
            builder = builder.Where(@$"b.isbn LIKE '%' || @{nameof(isbn)} || '%'", new { isbn });
        }
        if (categoryId.HasValue)
        {
            builder = builder.Where(@$"b.category_id = @{nameof(categoryId)}", new { categoryId });
        }
        if (!string.IsNullOrEmpty(cCode))
        {
            builder = builder.Where(@$"b.c_code = @{nameof(cCode)}", new { cCode });
        }
        if (ownerId.HasValue)
        {
            builder = builder.Where(@$"ub.user_id = @{nameof(ownerId)}", new { ownerId });
        }
        if (bookshelfId.HasValue) // Added
        {
            builder = builder.Where(@$"b.bookshelf_id = @{nameof(bookshelfId)}", new { bookshelfId });
        }
        if (readingStatus.HasValue)
        {
            builder = builder.Where(@$"ub.reading_status = @{nameof(readingStatus)}", new { readingStatus = readingStatus.ToString() });
        }

        return await QueryAsync(builder);
    }

    private async Task<BookSchema[]> QueryAsync(SqlBuilder builder)
    {
        var sql = builder.AddTemplate(Sql);
        var result = await _connection.QueryAsync<BookSchema, object, BookSchema>(
            sql.RawSql,
            (book, categoryObj) =>
            {
                book.Category = categoryObj.ToSchema<CategorySchema>("cat_"); ;
                return book;
            },
            sql.Parameters,
            splitOn: "cat_id",
            transaction: _transaction);
        return result.ToArray();
    }
}
