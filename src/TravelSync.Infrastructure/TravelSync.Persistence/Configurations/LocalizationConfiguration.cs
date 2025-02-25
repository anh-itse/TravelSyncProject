using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelSync.Domain.Constants;
using TravelSync.Domain.Entities;

namespace TravelSync.Persistence.Configurations;

public class LocalizationConfiguration : IEntityTypeConfiguration<Localization>
{
    public void Configure(EntityTypeBuilder<Localization> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(TableNames.Localizations);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnType("nvarchar(1000)")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.LanguageId)
            .IsRequired();

        builder.HasOne(x => x.Language)
            .WithMany(x => x.Localizations);
    }
}
