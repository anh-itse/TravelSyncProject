using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Persistence.Entities.Identity;

public class AppPermission : FullAuditableEntity<Guid>
{
    public Guid RoleId { get; set; }
    public Guid FunctionId { get; set; }
    public Guid ActionId { get; set; } 
}
