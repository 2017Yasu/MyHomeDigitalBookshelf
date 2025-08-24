using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Domain.Repositories;

public interface IUserIdentityRepository
{
    Task<UserIdentity?> GetByIdAsync(Guid id);
    Task<UserIdentity?> GetByProviderAndSubjectAsync(string provider, string subject);
    Task AddAsync(UserIdentity identity);
    Task DeleteAsync(Guid id);
}
