using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TravelSync.Domain.Abstractions.Entities;
using TravelSync.Domain.Abstractions.Identity;

namespace TravelSync.Persistence.Interceptors;

public class AuditInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        var context = eventData.Context;

        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added && entry.Entity is ICreateAuditable createAuditable)
            {
                createAuditable.CreatedAt = DateTime.UtcNow;
                createAuditable.CreatedBy ??= currentUser.Email ?? "System";
            }

            if (entry.State == EntityState.Modified && entry.Entity is IModifyAuditable modifyAuditable)
            {
                modifyAuditable.ModifiedAt = DateTime.UtcNow;
                modifyAuditable.ModifiedBy ??= currentUser.Email ?? "System";
            }

            if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable softDeletable)
            {
                // Chuyển trạng thái từ Deleted -> Modified và đánh dấu xóa mềm
                softDeletable.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
