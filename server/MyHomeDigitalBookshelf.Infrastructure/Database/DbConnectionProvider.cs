using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace MyHomeDigitalBookshelf.Infrastructure.Database;

public class DbConnectionProvider : IDisposable
{
    private readonly DbDataSource _dataSource;
    private bool _isDisposed;

    public DbConnectionProvider(DbSettings settings, ILoggerFactory loggerFactory)
    {
        _dataSource = new Npgsql.NpgsqlDataSourceBuilder(settings.ConnectionString)
            .UseLoggerFactory(loggerFactory)
            .Build();
    }

    public async Task<DbConnection> GetConnection()
    {
        var conn = _dataSource.CreateConnection();
        await conn.OpenAsync();
        return conn;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _dataSource.Dispose();
            }
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
