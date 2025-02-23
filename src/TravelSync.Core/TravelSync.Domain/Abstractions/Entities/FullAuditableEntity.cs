namespace TravelSync.Domain.Abstractions.Entities;

public abstract class FullAuditableEntity<TKey> : DomainEntity<TKey>, IFullAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}
