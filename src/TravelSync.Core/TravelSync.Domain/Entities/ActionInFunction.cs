using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Domain.Entities;

public class ActionInFunction : FullAuditableEntity<Guid>
{
    public Guid ActionId { get; set; }
    public Guid FunctionId { get; set; }
}
