using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelSync.Persistence.Constants;
using TravelSync.Persistence.Entities.Identity;

namespace TravelSync.Persistence.Configurations;

internal sealed class AppFunctionConfiguration : IEntityTypeConfiguration<AppFunction>
{
    public void Configure(EntityTypeBuilder<AppFunction> builder)
    {
        builder.ToTable(TableNames.AppFunctions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired(true);

        builder.Property(x => x.ParrentId)
            .HasMaxLength(50)
            .HasDefaultValue(null);

        builder.Property(x => x.CssClass)
            .HasMaxLength(50)
            .HasDefaultValue(null);

        builder.Property(x => x.Url)
            .HasMaxLength(50)
            .IsRequired(true);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(null);

        // Each User can have many Permission
        builder.HasMany<AppPermission>(e => e.Permissions)
            .WithOne()
            .HasForeignKey(e => e.FunctionId)
            .IsRequired();

        // Each User can have many ActionInFunction
        builder.HasMany<ActionInFunction>(e => e.ActionInFunctions)
            .WithOne()
            .HasForeignKey(e => e.FunctionId)
            .IsRequired();
    }
}
