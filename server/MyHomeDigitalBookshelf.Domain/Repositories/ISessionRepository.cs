using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id);
    Task<IEnumerable<Session>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Session session);
    Task DeleteAsync(Guid id);
}
