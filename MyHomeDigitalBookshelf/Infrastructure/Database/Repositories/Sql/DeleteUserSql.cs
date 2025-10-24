using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for deleting users from the database.
/// </summary>
internal class DeleteUserSql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = @"
        DELETE FROM users
        /**where**/";

    /// <summary>
    /// Executes the delete command for a specific user.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>The number of rows affected.</returns>
    internal async Task<int> ExecuteAsync(Guid id)
    {
        var builder = new Dapper.SqlBuilder()
            .Where("id = @id", new { id });

        var template = builder.AddTemplate(Sql);
        return await _connection.ExecuteAsync(
            template.RawSql,
            template.Parameters,
            _transaction);
    }
}
