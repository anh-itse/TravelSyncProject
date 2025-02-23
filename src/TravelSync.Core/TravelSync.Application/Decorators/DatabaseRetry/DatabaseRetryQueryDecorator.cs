using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Application.Decorators.DatabaseRetry;

public sealed class DatabaseRetryQueryDecorator<TQuery, TResult>(
        IQueryHandler<TQuery, TResult> handler,
        DatabaseRetryAttribute options
    ) : DatabaseRetryBase(options), IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        return await this.WrapExecutionAsync<TResult>(() => handler.HandleAsync(query, cancellationToken));
    }
}
