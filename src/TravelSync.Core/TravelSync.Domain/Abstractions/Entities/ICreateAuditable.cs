namespace TravelSync.Domain.Abstractions.Entities;

public interface ICreateAuditable
{
    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }
}
