using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Application.Decorators.DatabaseRetry;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class DatabaseRetryAttribute(int retryTimes = 3, int delayTimes = 0) : DecoratorAttribute
{
    public int RetryTimes { get; } = retryTimes;
    public int DelayTimes { get; } = delayTimes;

    public override Type GetDecoratorType(Type handlerType)
    {
        if (handlerType.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            return typeof(DatabaseRetryCommanDecorator<>);

        if (handlerType.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
            return typeof(DatabaseRetryCommanDecorator<,>);

        if (handlerType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            return typeof(DatabaseRetryQueryDecorator<,>);

        throw new InvalidOperationException($"Handler type {handlerType.Name} is not supported by {this.GetType().Name}.");
    }
}
