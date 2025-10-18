// Build configuration from command-line arguments
using Microsoft.Extensions.Configuration;
using Npgsql;

var config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

// Read configuration values (case-insensitive)
var host = config["host"];
var port = config["port"];
var user = config["user"];
var password = config["password"];
var directoryPath = config["dir"];
var database = config["database"];
var downArg = config["down"];
var fromArg = config["from"] ?? "0";
var toArg = config["to"];

if (!int.TryParse(fromArg, out var from))
{
    from = 0;
}
if (!int.TryParse(toArg, out var to))
{
    to = int.MaxValue;
}
if (!bool.TryParse(downArg, out var isDown))
{
    isDown = false;
}

// Validate required options
if (string.IsNullOrWhiteSpace(host) ||
    string.IsNullOrWhiteSpace(port) ||
    string.IsNullOrWhiteSpace(user) ||
    string.IsNullOrWhiteSpace(password) ||
    string.IsNullOrWhiteSpace(database) ||
    string.IsNullOrWhiteSpace(directoryPath) ||
    from < 0 ||
    to < from)
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  DbMigrator --host <hostname> --port <port>  --database <name> --user <username> --password <password> --dir <directory> [--down true] [--from <number>] [--to <number>]");
    Environment.Exit(1);
    return;
}

if (!Directory.Exists(directoryPath))
{
    Console.WriteLine($"Error: Directory not found: {directoryPath}");
    Environment.Exit(1);
    return;
}

var connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={database}";
// var sqlFiles = Directory.GetFiles(directoryPath, "*.sql")
//     .OrderBy(f => f)
//     .SkipWhile(f => !f.StartsWith(from.ToString().PadLeft(4, '0') + "_"))
//     .TakeWhile(f => !f.StartsWith(to.ToString().PadLeft(4, '0') + "_"))
//     .ToArray();

var sqlFiles = Directory.GetFiles(directoryPath, "*.sql")
    .OrderBy(f => f)
    .Select(f => new { FilePath = f, Index = int.Parse(Path.GetFileName(f).Substring(0, 4)) })
    .SkipWhile(x => x.Index < from)
    .TakeWhile(x => x.Index <= to)
    .Select(x => x.FilePath)
    .ToArray();

if (isDown)
{
    sqlFiles = [.. sqlFiles.Reverse()];
}

if (sqlFiles.Length == 0)
{
    Console.WriteLine("No .sql files found in the directory.");
    return;
}

Console.WriteLine($"Connecting to {host}:{port} as {user}...");

try
{
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    Console.WriteLine("Connection successful.");

    foreach (var file in sqlFiles)
    {
        Console.WriteLine($"\nExecuting {Path.GetFileName(file)}...");
        var sql = File.ReadAllText(file);

        try
        {
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"✅ Executed: {Path.GetFileName(file)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error executing {Path.GetFileName(file)}: {ex.Message}");
            Environment.Exit(1);
            return;
        }
    }

    Console.WriteLine("\nAll SQL files processed.");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
    Environment.Exit(1);
    return;
}
