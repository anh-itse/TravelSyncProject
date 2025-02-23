using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Application.Decorators.DatabaseRetry;

public sealed class DatabaseRetryCommanDecorator<TCommand>(
        ICommandHandler<TCommand> handler,
        DatabaseRetryAttribute options
    ) : DatabaseRetryBase(options), ICommandHandler<TCommand>
        where TCommand : ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        await this.WrapExecutionAsync(() => handler.HandleAsync(command, cancellationToken));
    }
}

public sealed class DatabaseRetryCommanDecorator<TCommand, TResult>(
        ICommandHandler<TCommand, TResult> handler,
        DatabaseRetryAttribute options
    ) : DatabaseRetryBase(options), ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
{
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        return await this.WrapExecutionAsync(() => handler.HandleAsync(command, cancellationToken));
    }
}