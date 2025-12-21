using System.Text.Json;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Schema;

public class BookSchema
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid BookshelfId { get; set; }
    public string? Authors { get; set; }
    public string? Isbn { get; set; }
    public string? Publisher { get; set; }
    public DateTime? PublishDate { get; set; }
    public string? CCode { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public CategorySchema? Category { get; set; }

    public Domain.Entities.Book ToEntity()
    {
        return new Domain.Entities.Book(
            id: Id,
            title: Title,
            bookshelfId: BookshelfId,
            authors: string.IsNullOrEmpty(Authors) ? [] : Authors.Split(','),
            isbn: Isbn != null ? new Domain.ValueObjects.Isbn(Isbn) : null,
            publisher: Publisher,
            publishDate: PublishDate,
            cCode: CCode != null ? new Domain.ValueObjects.CCode(CCode) : null,
            categoryId: CategoryId,
            coverImageUrl: CoverImageUrl,
            notes: Notes,
            createdAt: CreatedAt,
            updatedAt: UpdatedAt,
            category: Category?.ToEntity());
    }
}
