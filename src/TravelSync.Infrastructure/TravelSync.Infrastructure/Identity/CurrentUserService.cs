using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TravelSync.Domain.Abstractions.Identity;

namespace TravelSync.Infrastructure.Identity;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly AnonymousUser _anonymousUser = new();

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string? UserId => User?.Identity?.IsAuthenticated == true ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : _anonymousUser.UserId;
    public string? UserName => User?.Identity?.IsAuthenticated == true ? User.Identity.Name : _anonymousUser.UserName;
    public string? Email => User?.Identity?.IsAuthenticated == true ? User.FindFirst(ClaimTypes.Email)?.Value : _anonymousUser.Email;
    public IEnumerable<string> Roles => User?.Identity?.IsAuthenticated == true ? User.FindAll(ClaimTypes.Role).Select(c => c.Value) : _anonymousUser.Roles;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public IEnumerable<string> Permissions => User?.Identity?.IsAuthenticated == true
        ? User.FindAll("permission").Select(c => c.Value) // Hoặc thay "permission" bằng tên Claim thực tế
        : _anonymousUser.Permissions;

    public bool HasRole(string role) => Roles.Contains(role);
    public bool HasPermission(string permission) => Permissions.Contains(permission);
}
