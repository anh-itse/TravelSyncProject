using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Domain.Entities;

public class AppFunction : FullAuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ParrentId { get; set; } = string.Empty;
    public int? SortOrder { get; set; }
    public string CssClass { get; set; } = string.Empty;
    public bool? IsActive { get; set; }

    public virtual ICollection<AppPermission>? AppPermissions { get; }
    public virtual ICollection<ActionInFunction>? ActionInFunctions { get; }
}
