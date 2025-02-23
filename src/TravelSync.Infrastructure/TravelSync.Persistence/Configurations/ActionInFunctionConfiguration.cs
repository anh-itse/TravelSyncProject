using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelSync.Persistence.Constants;
using TravelSync.Persistence.Entities.Identity;

namespace TravelSync.Persistence.Configurations;

internal sealed class ActionInFunctionConfiguration : IEntityTypeConfiguration<ActionInFunction>
{
    public void Configure(EntityTypeBuilder<ActionInFunction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableNames.ActionInFunctions);

        builder.HasKey(x => new { x.ActionId, x.FunctionId });
    }
}
