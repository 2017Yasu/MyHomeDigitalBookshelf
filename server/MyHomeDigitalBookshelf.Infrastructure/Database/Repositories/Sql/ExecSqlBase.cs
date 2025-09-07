using System;
using System.Data.Common;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories.Sql;

internal abstract class ExecSqlBase
{
    protected readonly DbConnection _connection;
    protected readonly DbTransaction _transaction;

    public ExecSqlBase(DbConnection connection, DbTransaction transaction)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }
}
