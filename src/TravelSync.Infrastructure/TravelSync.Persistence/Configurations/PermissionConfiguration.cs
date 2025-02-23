using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelSync.Persistence.Constants;
using TravelSync.Persistence.Entities.Identity;

namespace TravelSync.Persistence.Configurations;

internal sealed class AppPermissionConfiguration : IEntityTypeConfiguration<AppPermission>
{
    public void Configure(EntityTypeBuilder<AppPermission> builder)
    {
        builder.ToTable(TableNames.AppPermissions);

        builder.HasKey(x => new { x.RoleId, x.FunctionId, x.ActionId });
    }
}