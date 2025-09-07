using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public class UserRepository(ILogger<UserRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id)
    {
        return QueryAndTraceAsync<User?>(conn => throw new NotImplementedException(), "Get User By Id", $"id: {id}");
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return QueryAndTraceAsync<User?>(conn => throw new NotImplementedException(), "Get User By Username", $"username: {username}");
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return QueryAndTraceAsync<User?>(conn => throw new NotImplementedException(), "Get User By Email", $"email: {email}");
    }

    public Task<User[]> GetAllAsync()
    {
        return QueryAndTraceAsync<User[]>(conn => throw new NotImplementedException(), "Get All Users");
    }

    public Task<User> AddAsync(User user)
    {
        return ExecuteAndTraceAsync<User>((conn, tran) => throw new NotImplementedException(), "Add User", user.ToString());
    }

    public Task<User?> UpdateAsync(User user)
    {
        return ExecuteAndTraceAsync<User?>((conn, tran) => throw new NotImplementedException(), "Update User", user.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync<int>((conn, tran) => throw new NotImplementedException(), "Delete User", $"id: {id}");
    }
}
