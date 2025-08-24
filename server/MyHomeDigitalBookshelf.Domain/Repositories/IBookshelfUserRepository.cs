using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

public interface IBookshelfUserRepository
{
    Task<BookshelfUser?> GetAsync(Guid userId, Guid bookshelfId);
    Task<IEnumerable<BookshelfUser>> GetByBookshelfAsync(Guid bookshelfId);
    Task<IEnumerable<BookshelfUser>> GetByUserAsync(Guid userId);
    Task AddAsync(BookshelfUser bookshelfUser);
    Task UpdateAsync(BookshelfUser bookshelfUser);
    Task DeleteAsync(Guid userId, Guid bookshelfId);
}
