using TravelSync.Domain.Abstractions.Entities;

namespace TravelSync.Domain.Entities;

public class Localization : FullAuditableEntity<Guid>
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public Guid LanguageId { get; set; }
    public Language? Language { get; }
}
