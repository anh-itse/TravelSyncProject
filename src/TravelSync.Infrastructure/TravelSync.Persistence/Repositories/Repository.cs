using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TravelSync.Domain.Abstractions.Repositories;
using TravelSync.Persistence.Abstractions;

namespace TravelSync.Persistence.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(IDbContextProvider dbContextProvider)
    {
        ArgumentNullException.ThrowIfNull(dbContextProvider);

        this._dbContext = dbContextProvider.CreateDbContext();
        this._dbSet = this._dbContext.Set<TEntity>();
    }

    public DbSet<TEntity> DbSet => this._dbSet;

    public ApplicationDbContext DbContext => this._dbContext;

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => await this._dbSet.FindAsync([id], cancellationToken);

    public async Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await this._dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public IQueryable<TEntity> GetQueryable() => this._dbSet;

    public async Task<List<TDto>> GetListAsync<TDto>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TDto>> selector,
        CancellationToken cancellationToken = default)
        => await this._dbSet.Where(predicate).Select(selector).ToListAsync(cancellationToken);

    public async Task<(List<TDto> Items, int TotalCount)> GetPagedListAsync<TDto>(
     IQueryable<TDto> query,
     int pageIndex,
     int pageSize,
     Expression<Func<TDto, object>>? orderBy = null,
     bool orderByDescending = true,
     CancellationToken cancellationToken = default)
    {
        if (orderBy != null)
            query = orderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await this._dbSet.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await this._dbSet.AddRangeAsync(entities, cancellationToken);

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this._dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TKey> InsertAndGetIdAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this._dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var keyProperty = _dbContext.Entry(entity).Property<TKey>("Id");
        return keyProperty.CurrentValue!;
    }

    public void Update(TEntity entity) => this._dbSet.Update(entity);

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this._dbSet.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity is null) return;

        this._dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var entities = await this._dbSet.Where(predicate).ToListAsync(cancellationToken);

        if (entities.Count == 0) return;

        this._dbSet.RemoveRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Delete(TKey id)
    {
        var entity = this._dbSet.Find(id);

        if (entity is null) return;

        this._dbSet.Remove(entity);
    }

    public void Delete(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = this._dbSet.Where(predicate).ToList();

        if (entities.Count == 0) return;

        this._dbSet.RemoveRange(entities);
    }
}
