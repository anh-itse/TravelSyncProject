using Microsoft.Extensions.Logging;
using TravelSync.Domain.Abstractions.Events;
using TravelSync.Domain.Helpers;
using TravelSync.Shared.Helpers;

namespace TravelSync.Application.Decorators.AuditLogs;

public sealed class LoggingDomainEventHandlerDecorator<TEvent>(
        IDomainEventHandler<TEvent> innerHandler,
        ILogger<LoggingDomainEventHandlerDecorator<TEvent>> logger
    ) : IDomainEventHandler<TEvent>
        where TEvent : IDomainEvent
{
    public async Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken)
    {
        LogHelper.Info(logger, $"[Handling] {typeof(TEvent).Name}: {domainEvent.ToJsonString()}.");
        await innerHandler.HandleAsync(domainEvent, cancellationToken);
        LogHelper.Info(logger, $"[Finished] {typeof(TEvent).Name}.");
    }
}
