using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for deleting sessions from the database.
/// </summary>
internal class DeleteSessionSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        DELETE FROM sessions
        /**where**/";

    /// <summary>
    /// Executes the delete command for a specific session.
    /// </summary>
    /// <param name="id">The ID of the session to delete.</param>
    /// <returns>The number of rows affected.</returns>
    internal async Task<int> ExecuteAsync(Guid id)
    {
        var builder = new SqlBuilder();
        builder.Where("id = @id", new { id });
        var template = builder.AddTemplate(Sql);

        return await _connection.ExecuteAsync(
            template.RawSql,
            template.Parameters,
            _transaction);
    }

    /// <summary>
    /// Deletes all expired sessions.
    /// </summary>
    /// <returns>The number of rows affected.</returns>
    internal async Task<int> DeleteExpiredAsync()
    {
        var builder = new SqlBuilder();
        builder.Where("expires_at < CURRENT_TIMESTAMP");
        var template = builder.AddTemplate(Sql);

        return await _connection.ExecuteAsync(
            template.RawSql,
            template.Parameters,
            _transaction);
    }
}
