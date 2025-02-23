using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelSync.Persistence.Constants;
using TravelSync.Persistence.Entities.Identity;

namespace TravelSync.Persistence.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable(TableNames.AppUsers);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsDirector).HasDefaultValue(false);
        builder.Property(x => x.IsHeadOfDepartment).HasDefaultValue(false);
        builder.Property(x => x.ManagerId).HasDefaultValue(null);
        builder.Property(x => x.IsReceipient).HasDefaultValue(-1);

        // Each User can have many UserClaims
        builder.HasMany<IdentityUserClaim<Guid>>(e => e.Claims)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany<IdentityUserLogin<Guid>>(e => e.Logins)
            .WithOne()
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany<IdentityUserToken<Guid>>(e => e.Tokens)
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany<IdentityUserRole<Guid>>(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}