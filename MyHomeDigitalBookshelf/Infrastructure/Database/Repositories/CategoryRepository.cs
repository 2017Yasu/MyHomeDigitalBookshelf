using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

[SingletonService]
public class CategoryRepository(ILogger<CategoryRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id)
    {
        var result = await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetCategoriesSql(conn).ExecuteSingleAsync(id);
                return schema?.ToEntity();
            },
            "Get Category By Id",
            $"id: {id}");

        return result;
    }

    public async Task<Category[]> GetAllByBookshelfAsync(Guid bookshelfId)
    {
        var results = await QueryAndTraceAsync(
            async conn =>
            {
                var schemas = await new GetCategoriesSql(conn).ExecuteAsync(bookshelfId);
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Get Categories By Bookshelf",
            $"bookshelfId: {bookshelfId}");

        return results;
    }

    public async Task<Category> AddAsync(Category category)
    {
        var result = await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new AddCategorySql(conn, tran).ExecuteAsync(category);
                return schema.ToEntity();
            },
            "Add Category",
            category.ToString());

        return result;
    }

    public async Task<Category?> UpdateAsync(Category category)
    {
        var result = await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new UpdateCategorySql(conn, tran).ExecuteAsync(category);
                return schema?.ToEntity();
            },
            "Update Category",
            category.ToString());

        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>(
            (conn, tran) => new DeleteCategorySql(conn, tran).ExecuteAsync(id),
            "Delete Category",
            $"id: {id}");
    }
}
