using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL queries for retrieving user identities from the database.
/// </summary>
internal class GetUserIdentitiesSql(DbConnection connection, DbTransaction? transaction = null)
    : QuerySqlBase(connection, transaction)
{
    private const string Sql = @"
        SELECT
            id,
            user_id,
            provider,
            subject,
            email,
            created_at
        FROM user_identities
        /**where**/
        /**orderby**/";

    /// <summary>
    /// Queries a single user identity by its ID.
    /// </summary>
    /// <param name="id">The user identity ID to search for.</param>
    /// <returns>The user identity schema if found, null otherwise.</returns>
    internal async Task<Schema.UserIdentitySchema?> QuerySingleAsync(Guid id)
    {
        var builder = new Dapper.SqlBuilder()
            .Where("id = @id", new { id })
            .OrderBy("created_at DESC");

        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Queries a user identity by provider and subject.
    /// </summary>
    /// <param name="provider">The provider name to search for.</param>
    /// <param name="subject">The subject identifier to search for.</param>
    /// <returns>The user identity schema if found, null otherwise.</returns>
    internal async Task<Schema.UserIdentitySchema?> QueryByProviderAndSubjectAsync(string provider, string subject)
    {
        var builder = new Dapper.SqlBuilder()
            .Where("provider = @provider", new { provider })
            .Where("subject = @subject", new { subject })
            .OrderBy("created_at DESC");

        var results = await QueryAsync(builder);
        return results.FirstOrDefault();
    }

    private async Task<Schema.UserIdentitySchema[]> QueryAsync(Dapper.SqlBuilder builder)
    {
        var template = builder.AddTemplate(Sql);
        var rows = await _connection.QueryAsync<Schema.UserIdentitySchema>(
            template.RawSql,
            template.Parameters,
            _transaction);
        return rows.ToArray();
    }
}
