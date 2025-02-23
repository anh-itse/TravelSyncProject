using Microsoft.Extensions.Logging;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Domain.Helpers;
using TravelSync.Shared.Helpers;

namespace TravelSync.Application.Decorators.AuditLogs;

public sealed class LoggingQueryHandlerDecorator<TQuery, TResult>(
        IQueryHandler<TQuery, TResult> innerHandler,
        ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger
    ) : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        LogHelper.Info(logger, $"[Handling] {typeof(TQuery).Name}: {query.ToJsonString()}");
        var result = await innerHandler.HandleAsync(query, cancellationToken);
        LogHelper.Info(logger, $"[Finished] {typeof(TQuery).Name}.");
        return result;
    }
}
