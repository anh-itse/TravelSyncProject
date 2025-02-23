namespace TravelSync.Domain.Abstractions.Entities;

public abstract class DomainEntity<TKey>
{
    required public virtual TKey Id { get; set; }

    /// <summary>
    /// True if the entity is transient, i.e. not yet persisted to the database.
    /// </summary>
    /// <returns>Returns true if the entity is transient; otherwise, false.</returns>
    public bool IsTransient() => EqualityComparer<TKey>.Default.Equals(this.Id, default);
}
