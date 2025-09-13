using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Infrastructure.Database;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public abstract class RepositoryTestBase
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
        var hostname = "127.0.0.1";
        var port = 35432;
#if TESTING
        _logger.LogDebug("Using TESTING database settings");
        hostname = "db_test";
        port = 5432;
#else
        _logger.LogDebug("Using LOCAL database settings");
#endif
        return new DbSettings(
            hostname,
            "my_home_bookshelves",
            "test_user",
            "test",
            port);
    }
}
