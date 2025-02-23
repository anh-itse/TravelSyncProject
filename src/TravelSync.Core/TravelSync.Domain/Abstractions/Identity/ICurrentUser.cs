namespace TravelSync.Domain.Abstractions.Identity;

public interface ICurrentUser
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    IEnumerable<string> Roles { get; }
    IEnumerable<string> Permissions { get; }
    bool IsAuthenticated { get; }

    bool HasRole(string role);
    bool HasPermission(string permission);
}
