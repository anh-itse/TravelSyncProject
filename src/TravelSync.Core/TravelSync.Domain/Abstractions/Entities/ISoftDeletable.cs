namespace TravelSync.Domain.Abstractions.Entities;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
