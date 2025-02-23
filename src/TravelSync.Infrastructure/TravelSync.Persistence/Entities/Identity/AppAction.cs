using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Persistence.Entities.Identity;

public class AppAction : FullAuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public int? SortOrder { get; set; }
    public bool? IsActive { get; set; }

    public virtual ICollection<AppPermission>? AppPermissions { get; }
    public virtual ICollection<ActionInFunction>? ActionInFunctions { get; }
}
