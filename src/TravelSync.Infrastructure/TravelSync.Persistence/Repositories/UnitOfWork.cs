using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using TravelSync.Domain.Abstractions;

namespace TravelSync.Persistence.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public IDbTransaction? GetCurrentTransaction()
    {
        return this._transaction?.GetDbTransaction();
    }

    public async Task<IUnitOfWork> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (this._transaction != null) return this;

        this._transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        return this;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(this._transaction);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await this._transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await this.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await this._transaction.DisposeAsync();
            this._transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(this._transaction);

        await this._transaction.RollbackAsync(cancellationToken);
        await this._transaction.DisposeAsync();
        this._transaction = null;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (this._disposed) return;

        this._disposed = true;

        if (!disposing || this._transaction is null) return;

        await this._transaction.DisposeAsync();
        this._transaction = null;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this._disposed) return;

        this._disposed = true;

        if (!disposing || this._transaction is null) return;

        this._transaction.Dispose();
        this._transaction = null;
    }
}
