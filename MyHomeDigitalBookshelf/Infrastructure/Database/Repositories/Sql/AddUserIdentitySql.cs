using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for adding new user identities to the database.
/// </summary>
internal class AddUserIdentitySql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        INSERT INTO user_identities (
            user_id,
            provider,
            subject,
            email)
        VALUES (
            @userId,
            @provider,
            @subject,
            @email)
        RETURNING
            id,
            user_id,
            provider,
            subject,
            email,
            created_at";

    /// <summary>
    /// Executes the insert command for a new user identity.
    /// </summary>
    /// <param name="identity">The user identity to insert.</param>
    /// <returns>The inserted user identity as a schema.</returns>
    internal async Task<Schema.UserIdentitySchema> ExecuteAsync(Domain.Entities.UserIdentity identity)
    {
        var result = await _connection.QuerySingleAsync<Schema.UserIdentitySchema>(
            Sql,
            new
            {
                userId = identity.UserId,
                provider = identity.Provider,
                subject = identity.Subject,
                email = identity.Email?.Value
            },
            _transaction);

        return result;
    }
}
