using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TravelSync.Domain.Abstractions.Entities;
using TravelSync.Domain.Entities;
using TravelSync.Persistence.Interceptors;

namespace TravelSync.Persistence;

public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditInterceptor auditInterceptor
    ) : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppAction> Actions { get; set; }
    public DbSet<AppFunction> Functions { get; set; }
    public DbSet<ActionInFunction> ActionInFunctions { get; set; }
    public DbSet<AppPermission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.OnModelCreating(builder);

        // Áp dụng cấu hình chung cho các Entity
        AuditableEntityConfiguration(builder);

        // Áp dụng tất cả cấu hình Entity từ Assembly
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

        // Thêm Query Filter tự động lọc dữ liệu đã xóa mềm
        ApplyGlobalQueryFilters(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder?.AddInterceptors(auditInterceptor);

    private static void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        var softDeletableEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .Where(entityType => typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            .Select(entityType => entityType.ClrType);

        foreach (var entityType in softDeletableEntityTypes)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var filter = Expression.Lambda(
                Expression.Equal(
                    Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted)),
                    Expression.Constant(false)
                ),
                parameter
            );

            modelBuilder.Entity(entityType).HasQueryFilter(filter);
        }
    }

    private static void AuditableEntityConfiguration(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => typeof(IAuditableEntity).IsAssignableFrom(e.ClrType)
                || typeof(IFullAuditableEntity).IsAssignableFrom(e.ClrType))
            .Select(entityType => entityType.ClrType);

        foreach (var entityType in entityTypes)
        {
            var builder = modelBuilder.Entity(entityType);

            // Áp dụng cấu hình cho thuộc tính audit chung
            builder.Property(nameof(IAuditableEntity.CreatedAt))
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(nameof(IAuditableEntity.CreatedBy))
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(nameof(IAuditableEntity.IsDeleted))
                .HasDefaultValue(false);

            // Nếu entity implement IFullAuditableEntity, áp dụng thêm cấu hình cho Modified
            if (typeof(IFullAuditableEntity).IsAssignableFrom(entityType))
            {
                builder.Property(nameof(IFullAuditableEntity.ModifiedAt))
                    .HasColumnType("datetime2")
                    .IsRequired(false);

                builder.Property(nameof(IFullAuditableEntity.ModifiedBy))
                    .HasMaxLength(100)
                    .IsUnicode(false);
            }
        }
    }
}
