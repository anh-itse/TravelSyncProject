using TravelSync.Domain.Abstractions.Events;
using TravelSync.Domain.Shared;

namespace TravelSync.Application.Abstractions.Dispatching;

public interface IDispatcher
{
    Task<OperationResult> DispatchAsync(ICommand command, CancellationToken cancellationToken = default);

    Task<OperationResult<TResult>> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

    Task<OperationResult<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

    Task<OperationResult> DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
