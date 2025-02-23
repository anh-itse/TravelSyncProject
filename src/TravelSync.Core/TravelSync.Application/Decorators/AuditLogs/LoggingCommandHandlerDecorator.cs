using Microsoft.Extensions.Logging;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Domain.Helpers;
using TravelSync.Shared.Helpers;

namespace TravelSync.Application.Decorators.AuditLogs;

public sealed class LoggingCommandHandlerDecorator<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<LoggingCommandHandlerDecorator<TCommand>> logger
    ) : ICommandHandler<TCommand>
        where TCommand : ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        LogHelper.Info(logger, $"[Handling] {typeof(TCommand).Name}.");
        await innerHandler.HandleAsync(command, cancellationToken);
        LogHelper.Info(logger, $"[Finished] {typeof(TCommand).Name}.");
    }
}

public sealed class LoggingCommandHandlerDecoratorResult<TCommand, TResult>(
        ICommandHandler<TCommand, TResult> innerHandler,
        ILogger<LoggingCommandHandlerDecoratorResult<TCommand, TResult>> logger
    ) : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
{
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        LogHelper.Info(logger, $"[Handling] {typeof(TCommand).Name}: {command.ToJsonString()}");
        var result = await innerHandler.HandleAsync(command, cancellationToken);
        LogHelper.Info(logger, $"[Finished] {typeof(TCommand).Name}.");
        return result;
    }
}
