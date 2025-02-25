using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TravelSync.Domain.Abstractions.Identity;

namespace TravelSync.Infrastructure.Identity;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly AnonymousUser _anonymousUser = new ();

    private ClaimsPrincipal? User => this._httpContextAccessor.HttpContext?.User;

    public string? UserId => this.User?.Identity?.IsAuthenticated == true ? this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value : this._anonymousUser.UserId;
    public string? UserName => this.User?.Identity?.IsAuthenticated == true ? this.User.Identity.Name : this._anonymousUser.UserName;
    public string? Email => this.User?.Identity?.IsAuthenticated == true ? this.User.FindFirst(ClaimTypes.Email)?.Value : this._anonymousUser.Email;
    public IEnumerable<string> Roles => this.User?.Identity?.IsAuthenticated == true ? this.User.FindAll(ClaimTypes.Role).Select(c => c.Value) : this._anonymousUser.Roles;
    public bool IsAuthenticated => this.User?.Identity?.IsAuthenticated == true;
    public IEnumerable<string> Permissions => this.User?.Identity?.IsAuthenticated == true
        ? this.User.FindAll("permission").Select(c => c.Value) // Hoặc thay "permission" bằng tên Claim thực tế
        : this._anonymousUser.Permissions;

    public bool HasRole(string role) => this.Roles.Contains(role);
    public bool HasPermission(string permission) => this.Permissions.Contains(permission);
}
