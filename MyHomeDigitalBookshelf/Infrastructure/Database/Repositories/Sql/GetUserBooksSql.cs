using System.Data.Common;
using Dapper;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal class GetUserBooksSql(DbConnection connection, DbTransaction? transaction = null)
    : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"
        SELECT ub.user_id, ub.book_id, ub.ownership, ub.reading_status,
               ub.loan_status, ub.purchase_date, ub.price,
               b.id AS b_id, b.title AS b_title, b.bookshelf_id AS b_bookshelf_id,
               b.authors AS b_authors, b.isbn AS b_isbn, b.publisher AS b_publisher,
               b.publish_date AS b_publish_date, b.c_code AS b_c_code,
               b.category_id AS b_category_id, b.cover_image_url AS b_cover_image_url,
               b.notes AS b_notes, b.created_at AS b_created_at, b.updated_at AS b_updated_at,
               c.id AS cat_id, c.name AS cat_name, c.description AS cat_description,
               c.created_at AS cat_created_at, c.updated_at AS cat_updated_at,
               u.id AS u_id, u.username AS u_username, u.email AS u_email,
               u.role AS u_role, u.created_at AS u_created_at
        FROM user_books ub
        LEFT JOIN books b ON b.id = ub.book_id
        LEFT JOIN categories c ON c.id = b.category_id
        LEFT JOIN users u ON u.id = ub.user_id
        /**where**/";

    public async Task<Schema.UserBookSchema?> QuerySingleAsync(Guid userId, Guid bookId)
    {
        var builder = new SqlBuilder()
            .Where(@"ub.user_id = @userId AND ub.book_id = @bookId",
                new { userId, bookId });
        return (await QueryAsync(builder)).FirstOrDefault();
    }

    public async Task<Schema.UserBookSchema[]> QueryByUserAsync(Guid userId)
    {
        var builder = new SqlBuilder()
            .Where(@"ub.user_id = @userId", new { userId });
        return await QueryAsync(builder);
    }

    public async Task<Schema.UserBookSchema[]> QueryByBookAsync(Guid bookId)
    {
        var builder = new SqlBuilder()
            .Where(@"ub.book_id = @bookId", new { bookId });
        return await QueryAsync(builder);
    }

    private async Task<Schema.UserBookSchema[]> QueryAsync(SqlBuilder builder)
    {
        var sql = builder.AddTemplate(Sql);
        var result = await _connection.QueryAsync<Schema.UserBookSchema, object, object, Schema.UserBookSchema>(
            sql.RawSql,
            (userBook, bookObj, userObj) =>
            {
                var book = bookObj.ToSchema<BookWithCategorySchema>("b_");
                var user = userObj.ToSchema<UserSchema>("u_");
                userBook.Book = book;
                userBook.User = user;
                return userBook;
            },
            sql.Parameters,
            splitOn: "b_id,u_id",
            transaction: _transaction);
        return result.ToArray();
    }
}
