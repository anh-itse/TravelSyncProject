using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Application.Decorators.AutoRetry;

public sealed class AutoRetryCommanDecorator<TCommand>(
        ICommandHandler<TCommand> handler,
        AutoRetryAttribute options
    ) : AutoRetryBase(options), ICommandHandler<TCommand>
        where TCommand : ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        => await this.WrapExecutionAsync(() => handler.HandleAsync(command, cancellationToken));
}

public sealed class AutoRetryCommanDecorator<TCommand, TResult>(
        ICommandHandler<TCommand, TResult> handler,
        AutoRetryAttribute options
    ) : AutoRetryBase(options), ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
{
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        => await this.WrapExecutionAsync(() => handler.HandleAsync(command, cancellationToken));
}