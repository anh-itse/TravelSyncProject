using Microsoft.AspNetCore.Identity;
using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Domain.Entities;

public class AppUser : IdentityUser<Guid>, ICreateAuditable, IModifyAuditable, ISoftDeletable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public bool? IsDirector { get; set; }
    public bool? IsHeadOfDepartment { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid PositionId { get; set; }
    public int IsReceipient { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>>? Claims { get; }
    public virtual ICollection<IdentityUserLogin<Guid>>? Logins { get; }
    public virtual ICollection<IdentityUserToken<Guid>>? Tokens { get; }
    public virtual ICollection<IdentityUserRole<Guid>>? UserRoles { get; }
}
