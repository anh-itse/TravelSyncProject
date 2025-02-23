using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TravelSync.Domain.Abstractions.Dapper;
using TravelSync.Domain.Constants;

namespace TravelSync.Persistence.Dapper;

public class DapperQueryExecutor(ApplicationDbContext dbContext) : IDapperQueryExecutor, IDisposable
{
    private readonly DbConnection _connection = dbContext.Database.GetDbConnection();
    private int _disposed;

    public async Task<List<T>> QueryAsync<T>(
        string sql,
        object? param = null,
        CommandType commandType = CommandType.StoredProcedure,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        sql = GetNameStoredProcedure(sql, commandType);
        return (await this.ExecuteWithTransactionAsync(conn => conn.QueryAsync<T>(sql, param, transaction, commandType: commandType), transaction)).AsList();
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? param = null,
        CommandType commandType = CommandType.StoredProcedure,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        sql = GetNameStoredProcedure(sql, commandType);
        return await this.ExecuteWithTransactionAsync(conn => conn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandType: commandType), transaction);
    }

    public async Task<T> QuerySingleAsync<T>(
        string sql,
        object? param = null,
        CommandType commandType = CommandType.StoredProcedure,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        sql = GetNameStoredProcedure(sql, commandType);
        return await this.ExecuteWithTransactionAsync(conn => conn.QuerySingleAsync<T>(sql, param, transaction, commandType: commandType), transaction);
    }

    public async Task<int> ExecuteAsync(
        string sql,
        object? param = null,
        CommandType commandType = CommandType.StoredProcedure,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        sql = GetNameStoredProcedure(sql, commandType);
        return await this.ExecuteWithTransactionAsync(conn => conn.ExecuteAsync(sql, param, transaction, commandType: commandType), transaction);
    }

    public IDbConnection GetDbConnection() => this._connection;

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref this._disposed, 1) == 1) return;

        if (disposing) this._connection.Dispose();
    }

    private static string GetNameStoredProcedure(string sql, CommandType commandType)
    {
        return (commandType == CommandType.StoredProcedure && !string.IsNullOrEmpty(DbConst.Configurations.DbSchema))
            ? $"{DbConst.Configurations.DbSchema}.{sql}"
            : sql;
    }

    private async Task<T> ExecuteWithTransactionAsync<T>(Func<IDbConnection, Task<T>> query, IDbTransaction? transaction)
    {
        if (transaction != null) return await query(this._connection);

        using var localTransaction = await this._connection.BeginTransactionAsync();
        try
        {
            var result = await query(this._connection);
            await localTransaction.CommitAsync();
            return result;
        }
        catch
        {
            await localTransaction.RollbackAsync();
            throw;
        }
    }
}
