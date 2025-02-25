using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Domain.Entities;

public class Language : FullAuditableEntity<Guid>
{
    public string CultureCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Localization> Localizations { get; } = [];
}
