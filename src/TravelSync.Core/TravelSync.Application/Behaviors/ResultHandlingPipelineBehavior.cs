using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Domain.Abstractions.Events;
using TravelSync.Domain.Shared;
using TravelSync.Shared.Helpers;

namespace TravelSync.Application.Behaviors;

public class ResultHandlingPipelineBehavior(
        IDispatcher innerDispatcher,
        ILogger<ResultHandlingPipelineBehavior> logger
    ) : IDispatcher
{
    public Task<OperationResult> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
        => this.HandleDispatchAsync(() => innerDispatcher.DispatchAsync(command, cancellationToken), command);

    public Task<OperationResult<TResult>> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        => this.HandleDispatchAsync(() => innerDispatcher.DispatchAsync(command, cancellationToken), command);

    public Task<OperationResult<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => this.HandleDispatchAsync(() => innerDispatcher.DispatchAsync(query, cancellationToken), query);

    public Task<OperationResult> DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        => this.HandleDispatchAsync(() => innerDispatcher.DispatchAsync(domainEvent, cancellationToken), domainEvent);

    private async Task<OperationResult<T>> HandleDispatchAsync<T>(Func<Task<OperationResult<T>>> action, object? param = null)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            var className = action.Method.DeclaringType?.Name ?? "UnknownClass";
            var paramInfo = param != null ? $"Param: {JsonConvert.SerializeObject(param)}" : "No Params";

            LogHelper.Error(logger, ex, $"[Error] [{className}]: {paramInfo}.");
            return OperationResult<T>.Failure(ErrorDetail.BadRequest.WithMessage(ex.Message));
        }
    }

    private async Task<OperationResult> HandleDispatchAsync(Func<Task<OperationResult>> action, object? param = null)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            var className = action.Method.DeclaringType?.Name ?? "UnknownClass";
            var paramInfo = param != null ? $"Param: {JsonConvert.SerializeObject(param)}" : "No Params";

            LogHelper.Error(logger, ex, $"[Error] [{className}]: {paramInfo}.");
            return OperationResult.Failure(ErrorDetail.BadRequest.WithMessage(ex.Message));
        }
    }
}
