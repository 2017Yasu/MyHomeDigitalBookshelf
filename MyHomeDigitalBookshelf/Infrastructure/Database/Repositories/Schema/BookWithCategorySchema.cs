using System;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

public class BookWithCategorySchema : BookSchema
{
    public string? CatName { get; set; }
    public string? CatDescription { get; set; }
    public string? CatCreatedAt { get; set; }
    public string? CatUpdatedAt { get; set; }

    public Domain.Entities.Book ToEntity()
    {
        var category = CategoryId.HasValue && !string.IsNullOrEmpty(CatName)
            ? new Domain.Entities.Category(
                id: CategoryId.Value,
                name: CatName,
                description: CatDescription,
                bookshelfId: BookshelfId,
                createdAt: DateTime.Parse(CatCreatedAt!),
                updatedAt: DateTime.Parse(CatUpdatedAt!))
            : null;

        return base.ToEntity(category);
    }
}
