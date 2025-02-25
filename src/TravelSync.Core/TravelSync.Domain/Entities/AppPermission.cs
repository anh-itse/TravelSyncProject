using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Domain.Entities;

public class AppPermission : FullAuditableEntity<Guid>
{
    public Guid RoleId { get; set; }
    public Guid FunctionId { get; set; }
    public Guid ActionId { get; set; }
}
