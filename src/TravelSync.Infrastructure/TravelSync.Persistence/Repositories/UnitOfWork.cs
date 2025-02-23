using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TravelSync.Domain.Abstractions;

namespace TravelSync.Persistence.Repositories;

public class UnitOfWork(DbContext context) : IUnitOfWork
{
    private readonly DbContext _context = context;
    private IDbContextTransaction? _transaction;

    public IDbTransaction? CurrentTransaction => this._transaction?.GetDbTransaction();

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (this._transaction != null) return;

        this._transaction = await this._context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(this._transaction);

        try
        {
            await this._context.SaveChangesAsync(cancellationToken);
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
        return await this._context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        ArgumentNullException.ThrowIfNull(this._transaction);

        await this._transaction.DisposeAsync();
        this._transaction = null;

        await this._context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}