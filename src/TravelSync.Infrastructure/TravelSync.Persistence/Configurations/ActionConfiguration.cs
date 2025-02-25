using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelSync.Domain.Constants;
using TravelSync.Domain.Entities;

namespace TravelSync.Persistence.Configurations;

internal sealed class ActionConfiguration : IEntityTypeConfiguration<AppAction>
{
    public void Configure(EntityTypeBuilder<AppAction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableNames.AppActions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.SortOrder).HasDefaultValue(null);

        // Each User can have many Permission
        builder.HasMany<AppPermission>(e => e.AppPermissions)
            .WithOne()
            .HasForeignKey(s => s.ActionId)
            .IsRequired();

        // Each user can have many ActionInFunction
        builder.HasMany<ActionInFunction>(x => x.ActionInFunctions)
            .WithOne()
            .HasForeignKey(x => x.ActionId)
            .IsRequired();
    }
}
