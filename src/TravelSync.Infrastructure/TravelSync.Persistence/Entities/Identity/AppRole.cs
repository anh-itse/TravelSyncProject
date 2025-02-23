using Microsoft.AspNetCore.Identity;
using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Persistence.Entities.Identity;

public class AppRole : IdentityRole<Guid>, ICreateAuditable, IModifyAuditable, ISoftDeletable
{
    public string? Description { get; set; }
    public string RoleCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    public virtual ICollection<IdentityUserRole<Guid>>? UserRoles { get; }
    public virtual ICollection<IdentityRoleClaim<Guid>>? Claims { get; }
    public virtual ICollection<AppPermission>? Permissions { get; }

}
