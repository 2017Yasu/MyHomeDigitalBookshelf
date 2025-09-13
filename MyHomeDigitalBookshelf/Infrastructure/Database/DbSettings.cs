namespace MyHomeDigitalBookshelf.Infrastructure.Database;

public class DbSettings
{
    public DbSettings(
        string host,
        string database,
        string username,
        string password,
        int port = 5432,
        int connectionTimeoutSeconds = 15,
        int commandTimeoutSeconds = 30)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            throw new ArgumentException("Host must not be null or whitespace.", nameof(host));
        }
        if (string.IsNullOrWhiteSpace(database))
        {
            throw new ArgumentException("Database must not be null or whitespace.", nameof(database));
        }
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username must not be null or whitespace.", nameof(username));
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password must not be null or whitespace.", nameof(password));
        }
        if (port <= 0 || port > 65535)
        {
            throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535.");
        }
        if (connectionTimeoutSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(connectionTimeoutSeconds), "ConnectionTimeoutSeconds must be non-negative.");
        }
        if (commandTimeoutSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(commandTimeoutSeconds), "CommandTimeoutSeconds must be non-negative.");
        }

        Host = host;
        Database = database;
        Username = username;
        Password = password;
        Port = port;
        ConnectionTimeoutSeconds = connectionTimeoutSeconds;
        CommandTimeoutSeconds = commandTimeoutSeconds;
    }

    public string ConnectionString
    {
        get
        {
            var builder = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Host = Host,
                Username = Username,
                Password = Password,
                Database = Database,
                Port = Port,
                Timeout = ConnectionTimeoutSeconds,
                CommandTimeout = CommandTimeoutSeconds,
                Pooling = true,
            };
            return builder.ConnectionString;
        }
    }
    public string Host { get; }
    public string Database { get; }
    public string Username { get; }
    public string Password { get; }
    public int Port { get; }
    public int ConnectionTimeoutSeconds { get; }
    public int CommandTimeoutSeconds { get; }
}
