using Microsoft.Extensions.DependencyInjection;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Domain.Abstractions.Events;
using TravelSync.Domain.Shared;

namespace TravelSync.Infrastructure.Dispatching;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<OperationResult> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command), "Command cannot be null.");

        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod("HandleAsync")
            ?? throw new InvalidOperationException($"Handler {handlerType.Name} does not implement HandleAsync");

        await (Task)method.Invoke(handler, [command, cancellationToken])!;

        return OperationResult.Success();
    }

    public async Task<OperationResult<TResult>> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command), "Command cannot be null.");

        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod("HandleAsync")
            ?? throw new InvalidOperationException($"Handler {handlerType.Name} does not implement HandleAsync");

        var result = await (Task<TResult>)method.Invoke(handler, [command, cancellationToken])!;

        return OperationResult<TResult>.Success(result);
    }

    public async Task<OperationResult<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        if (query is null) throw new ArgumentNullException(nameof(query), "Query cannot be null.");

        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod("HandleAsync")
            ?? throw new InvalidOperationException($"Handler {handlerType.Name} does not implement HandleAsync");

        var result = await (Task<TResult>)method.Invoke(handler, [query, cancellationToken])!;

        return OperationResult<TResult>.Success(result);
    }

    public async Task<OperationResult> DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        if (domainEvent is null) throw new ArgumentNullException(nameof(domainEvent), "Command cannot be null.");

        using var scope = serviceProvider.CreateScope();

        var handlerTypes = scope.ServiceProvider.GetServices(typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType()));

        if (!handlerTypes.Any())
            throw new InvalidOperationException($"No event handler found for event {domainEvent.GetType().Name}");

        var tasks = handlerTypes.Select(async handler =>
        {
            var handlerType = handler!.GetType();
            var method = handlerType.GetMethod("HandleAsync")
                ?? throw new InvalidOperationException($"Handler {handlerType.Name} does not implement HandleAsync");

            await (Task)method.Invoke(handler, [domainEvent, cancellationToken])!;
        });

        await Task.WhenAll(tasks);

        return OperationResult.Success();
    }
}
