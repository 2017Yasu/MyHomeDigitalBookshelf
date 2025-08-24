using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

public interface IBookshelfRepository
{
    Task<Bookshelf?> GetByIdAsync(Guid id);
    Task<IEnumerable<Bookshelf>> GetAllAsync();
    Task AddAsync(Bookshelf bookshelf);
    Task UpdateAsync(Bookshelf bookshelf);
    Task DeleteAsync(Guid id);
}
