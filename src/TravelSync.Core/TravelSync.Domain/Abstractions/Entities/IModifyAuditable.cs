namespace TravelSync.Domain.Abstractions.Entities;

public interface IModifyAuditable
{
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}
