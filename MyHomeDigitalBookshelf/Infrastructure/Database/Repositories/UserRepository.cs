using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

/// <summary>
/// Repository implementation for managing users in the PostgreSQL database.
/// </summary>
[SingletonService]
public class UserRepository(ILogger<UserRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IUserRepository
{
    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetUsersSql(conn).QuerySingleAsync(id);
                return schema?.ToEntity();
            },
            "Get User By Id",
            $"id: {id}");
    }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetUsersSql(conn).QueryByUsernameAsync(username);
                return schema?.ToEntity();
            },
            "Get User By Username",
            $"username: {username}");
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetUsersSql(conn).QueryByEmailAsync(email);
                return schema?.ToEntity();
            },
            "Get User By Email",
            $"email: {email}");
    }

    /// <inheritdoc />
    public async Task<User[]> GetAllAsync()
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schemas = await new GetUsersSql(conn).QueryAllAsync();
                return schemas.Select(s => s.ToEntity()).ToArray();
            },
            "Get All Users");
    }

    /// <inheritdoc />
    public async Task<User> AddAsync(User user)
    {
        return await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new AddUserSql(conn, tran).ExecuteAsync(user);
                return schema.ToEntity();
            },
            "Add User",
            user.ToString());
    }

    /// <inheritdoc />
    public async Task<User?> UpdateAsync(User user)
    {
        return await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new UpdateUserSql(conn, tran).ExecuteAsync(user);
                return schema?.ToEntity();
            },
            "Update User",
            user.ToString());
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync(
            async (conn, tran) => await new DeleteUserSql(conn, tran).ExecuteAsync(id),
            "Delete User",
            $"id: {id}");
    }
}
