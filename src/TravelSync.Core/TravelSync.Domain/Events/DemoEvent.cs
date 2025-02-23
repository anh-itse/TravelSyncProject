using TravelSync.Domain.Abstractions.Events;

namespace TravelSync.Domain.Events;

public static class DemoEvent
{
    public record DemoCreated(int Id) : IDomainEvent;
}
