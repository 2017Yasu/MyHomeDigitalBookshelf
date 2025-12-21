using System.Data.Common;
using Dapper;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

/// <summary>
/// SQL commands for deleting user identities from the database.
/// </summary>
internal class DeleteUserIdentitySql(DbConnection connection, DbTransaction transaction)
    : ExecSqlBase(connection, transaction)
{
    private const string Sql = "DELETE FROM user_identities WHERE id = @id";

    /// <summary>
    /// Executes the delete command for a user identity.
    /// </summary>
    /// <param name="id">The ID of the user identity to delete.</param>
    /// <returns>The number of rows affected.</returns>
    internal async Task<int> ExecuteAsync(Guid id)
    {
        return await _connection.ExecuteAsync(Sql, new { id }, _transaction);
    }
}
