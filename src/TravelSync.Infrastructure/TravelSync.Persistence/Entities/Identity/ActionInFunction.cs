using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Persistence.Entities.Identity;

public class ActionInFunction : FullAuditableEntity<Guid>
{
    public Guid ActionId { get; set; }
    public Guid FunctionId { get; set; } 
}
