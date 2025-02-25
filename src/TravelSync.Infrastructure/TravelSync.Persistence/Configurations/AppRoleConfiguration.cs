using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelSync.Domain.Constants;
using TravelSync.Domain.Entities;

namespace TravelSync.Persistence.Configurations;

internal sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableNames.AppRoles);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.RoleCode)
            .HasMaxLength(50)
            .IsRequired();

        // Each user can have many RoleClaims
        builder.HasMany<IdentityRoleClaim<Guid>>(e => e.Claims)
            .WithOne()
            .HasForeignKey(e => e.RoleId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany<IdentityUserRole<Guid>>(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        // Each User can have many Permission
        builder.HasMany<AppPermission>(e => e.Permissions)
            .WithOne()
            .HasForeignKey(p => p.RoleId)
            .IsRequired();
    }
}
