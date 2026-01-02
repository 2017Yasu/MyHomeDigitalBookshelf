namespace MyHomeDigitalBookshelf.Api.Settings;

public class DbSettings
{
    public string Host { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? Port { get; set; }
    public int? ConnectionTimeoutSeconds { get; set; }
    public int? CommandTimeoutSeconds { get; set; }

    public Infrastructure.Database.DbSettings ToInfrastructureDbSettings()
    {
        return new Infrastructure.Database.DbSettings(
            host: Host,
            database: Database,
            username: Username,
            password: Password,
            port: Port,
            connectionTimeoutSeconds: ConnectionTimeoutSeconds,
            commandTimeoutSeconds: CommandTimeoutSeconds
        );
    }
}
