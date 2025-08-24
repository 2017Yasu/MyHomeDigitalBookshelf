using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

public interface IUserBookRepository
{
    Task<UserBook?> GetAsync(Guid userId, Guid bookId);
    Task<IEnumerable<UserBook>> GetByUserAsync(Guid userId);
    Task<IEnumerable<UserBook>> GetByBookAsync(Guid bookId);
    Task AddAsync(UserBook userBook);
    Task UpdateAsync(UserBook userBook);
    Task DeleteAsync(Guid userId, Guid bookId);
}
