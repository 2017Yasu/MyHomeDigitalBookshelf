using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Infrastructure.Database;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public abstract class RepositoryTestBase : IAsyncDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly LoggerFactory _loggerFactory;
    private readonly ILogger<RepositoryTestBase> _logger;
    private DbConnectionProvider? _dbConnectionProvider;

    public RepositoryTestBase(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;

        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(_outputHelper));
        _logger = _loggerFactory.CreateLogger<RepositoryTestBase>();
    }

    protected ILogger<T> CreateLogger<T>()
    {
        return _loggerFactory.CreateLogger<T>();
    }

    protected DbConnectionProvider GetConnectionProvider()
    {
        if (_dbConnectionProvider is not null)
        {
            return _dbConnectionProvider;
        }

        _dbConnectionProvider = new DbConnectionProvider(GetDbSettings(), _loggerFactory);
        return _dbConnectionProvider;
    }

    protected DbSettings GetDbSettings()
    {
        return new DbSettings(
            "127.0.0.1",
            "my_home_bookshelves",
            "test_user",
            "test",
            35432);
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_dbConnectionProvider is not null)
        {
            using var conn = await _dbConnectionProvider.GetConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();

            // Set autocommit to true since we're doing DDL
            cmd.CommandText = @"
                TRUNCATE TABLE sessions, bookshelf_users, user_books, user_identities, users, books, categories, bookshelves RESTART IDENTITY CASCADE;";

            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }
    }
}
