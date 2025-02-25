using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Application.Decorators.AutoRetry;

public sealed class AutoRetryQueryDecorator<TQuery, TResult>(
        IQueryHandler<TQuery, TResult> handler,
        AutoRetryAttribute options
    ) : AutoRetryBase(options), IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
        => await this.WrapExecutionAsync<TResult>(() => handler.HandleAsync(query, cancellationToken));
}
