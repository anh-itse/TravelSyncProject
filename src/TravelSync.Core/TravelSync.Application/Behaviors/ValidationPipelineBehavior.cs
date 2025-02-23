using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Domain.Abstractions.Events;
using TravelSync.Domain.Shared;

namespace TravelSync.Application.Behaviors;

public class ValidationPipelineBehavior(
        IDispatcher innerDispatcher,
        IServiceProvider serviceProvider
    ) : IDispatcher
{
    public async Task<OperationResult> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        await this.ValidateAsync(command, cancellationToken);
        return await innerDispatcher.DispatchAsync(command, cancellationToken);
    }

    public async Task<OperationResult<TResult>> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        await this.ValidateAsync(command, cancellationToken);
        return await innerDispatcher.DispatchAsync(command, cancellationToken);
    }

    public async Task<OperationResult<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        await this.ValidateAsync(query, cancellationToken);
        return await innerDispatcher.DispatchAsync(query, cancellationToken);
    }

    public async Task<OperationResult> DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        return await innerDispatcher.DispatchAsync(domainEvent, cancellationToken);
    }

    private async Task ValidateAsync<T>(T request, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var validators = scope.ServiceProvider.GetServices<IValidator<T>>();

        if (!validators.Any()) return;

        var context = new ValidationContext<T>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }
    }
}