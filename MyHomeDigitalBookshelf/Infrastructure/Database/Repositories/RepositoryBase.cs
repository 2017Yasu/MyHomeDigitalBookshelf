using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;

public abstract class RepositoryBase
{
    private readonly DbConnectionProvider _connectionProvider;

    protected ILogger Logger { get; }

    public RepositoryBase(ILogger logger, DbConnectionProvider connectionProvider)
    {
        Logger = logger;
        _connectionProvider = connectionProvider;
    }

    protected async Task<T> QueryAndTraceAsync<T>(Func<DbConnection, Task<T>> queryFunc, string operationDescription, string parameters = "")
    {
        try
        {
            LogOperation(operationDescription, parameters);
            using var conn = await _connectionProvider.GetConnection();
            return await queryFunc(conn);
        }
        catch (Npgsql.NpgsqlException sqlEx)
        {
            LogSqlError(operationDescription, sqlEx);
            throw;
        }
        catch (Exception ex)
        {
            LogError(operationDescription, ex);
            throw;
        }
        finally
        {
            LogCompletion(operationDescription);
        }
    }

    protected async Task<T> ExecuteAndTraceAsync<T>(Func<DbConnection, DbTransaction, Task<T>> executeFunc, string operationDescription, string parameters = "")
    {
        try
        {
            LogOperation(operationDescription, parameters);
            using var conn = await _connectionProvider.GetConnection();
            using var transaction = await conn.BeginTransactionAsync();
            try
            {
                var result = await executeFunc(conn, transaction);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Npgsql.NpgsqlException sqlEx)
        {
            LogSqlError(operationDescription, sqlEx);
            if (sqlEx.ErrorCode == -2147467259) // Unique violation
            {
                throw new Application.Common.Exceptions.DuplicateEntityException(
                    $"A duplicate entity was detected during the operation {operationDescription}.",
                    sqlEx);
            }
            throw;
        }
        catch (Exception ex)
        {
            LogError(operationDescription, ex);
            throw;
        }
        finally
        {
            LogCompletion(operationDescription);
        }
    }

    private void LogOperation(string operationDescription, string parameters = "")
    {
        Logger.LogInformation("Starting operation: {operation} with parameters: {parameters}", operationDescription, parameters);
    }

    private void LogCompletion(string operationDescription)
    {
        Logger.LogInformation("Completed operation: {operation}", operationDescription);
    }

    private void LogError(string operationDescription, Exception ex)
    {
        Logger.LogError(ex, "Error during {operation}: {error}", operationDescription, ex.Message);
    }

    private void LogSqlError(string operationDescription, Npgsql.NpgsqlException ex)
    {
        Logger.LogError(ex, "SQL Error during {operation}: {error}, SQLState: {sqlState}, ErrorCode: {errorCode}", operationDescription, ex.Message, ex.SqlState, ex.ErrorCode);
    }
}
