using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Domain.Abstractions.Events;

namespace TravelSync.Application.Decorators.AuditLogs;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class AuditLogAttribute : DecoratorAttribute
{
    public override Type GetDecoratorType(Type handlerType)
    {
        if (handlerType.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            return typeof(LoggingCommandHandlerDecorator<>);

        if (handlerType.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
            return typeof(LoggingCommandHandlerDecoratorResult<,>);

        if (handlerType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            return typeof(LoggingQueryHandlerDecorator<,>);

        if (handlerType.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
            return typeof(LoggingDomainEventHandlerDecorator<>);

        throw new InvalidOperationException($"Handler type {handlerType.Name} is not supported by {this.GetType().Name}.");
    }
}
