using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Persistence.Entities;

public class Product : FullAuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}
