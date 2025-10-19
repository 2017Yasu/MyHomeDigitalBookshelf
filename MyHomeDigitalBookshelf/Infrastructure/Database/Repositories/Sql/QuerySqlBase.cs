using System;
using System.Data.Common;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal abstract class QuerySqlBase
{
    protected readonly DbConnection _connection;
    protected readonly DbTransaction? _transaction;

    public QuerySqlBase(DbConnection connection, DbTransaction? transaction)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _transaction = transaction;
    }
}
