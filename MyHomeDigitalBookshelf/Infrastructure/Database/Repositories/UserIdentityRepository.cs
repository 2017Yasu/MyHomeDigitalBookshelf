using System.Data.Common;
using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

/// <summary>
/// Repository implementation for managing user identities in the PostgreSQL database.
/// </summary>
[SingletonService]
public class UserIdentityRepository(ILogger<UserIdentityRepository> logger, DbConnectionProvider connectionProvider)
    : RepositoryBase(logger, connectionProvider), IUserIdentityRepository
{
    /// <inheritdoc />
    public async Task<UserIdentity?> GetByIdAsync(Guid id)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetUserIdentitiesSql(conn).QuerySingleAsync(id);
                return schema?.ToEntity();
            },
            "Get UserIdentity By Id",
            $"id: {id}");
    }

    /// <inheritdoc />
    public async Task<UserIdentity?> GetByProviderAndSubjectAsync(string provider, string subject)
    {
        return await QueryAndTraceAsync(
            async conn =>
            {
                var schema = await new GetUserIdentitiesSql(conn).QueryByProviderAndSubjectAsync(provider, subject);
                return schema?.ToEntity();
            },
            "Get UserIdentity By Provider/Subject",
            $"provider: {provider}, subject: {subject}");
    }

    /// <inheritdoc />
    public async Task<UserIdentity> AddAsync(UserIdentity identity)
    {
        return await ExecuteAndTraceAsync(
            async (conn, tran) =>
            {
                var schema = await new AddUserIdentitySql(conn, tran).ExecuteAsync(identity);
                return schema.ToEntity();
            },
            "Add UserIdentity",
            identity.ToString());
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        _ = await ExecuteAndTraceAsync(
            async (conn, tran) => await new DeleteUserIdentitySql(conn, tran).ExecuteAsync(id),
            "Delete UserIdentity",
            $"id: {id}");
    }
}
