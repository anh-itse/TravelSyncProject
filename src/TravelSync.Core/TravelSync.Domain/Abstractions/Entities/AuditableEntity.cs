namespace TravelSync.Domain.Abstractions.Entities;

public abstract class AuditableEntity<TKey> : DomainEntity<TKey>, IAuditableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
}