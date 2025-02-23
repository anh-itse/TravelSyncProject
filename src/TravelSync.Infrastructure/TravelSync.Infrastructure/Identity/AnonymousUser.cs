using TravelSync.Domain.Abstractions.Identity;

namespace TravelSync.Infrastructure.Identity;

public class AnonymousUser : ICurrentUser
{
    public string? UserId => null;
    public string? UserName => "Anonymous";
    public string? Email => null;
    public IEnumerable<string> Roles => [];
    public bool IsAuthenticated => false;

    public IEnumerable<string> Permissions => [];

    public bool HasPermission(string permission) => false;

    public bool HasRole(string role) => false;
}
