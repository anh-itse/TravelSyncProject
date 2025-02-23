using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Application.Decorators.Cache;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class CacheAttribute(int duration, params string[] keyProperties) : DecoratorAttribute
{
    public int Duration { get; } = duration;
    public string[] KeyProperties { get; } = keyProperties;

    public override Type GetDecoratorType(Type handlerType)
    {
        if (handlerType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            return typeof(CachedQueryHandlerDecorator<,>);

        throw new InvalidOperationException($"Handler type {handlerType.Name} is not supported by {this.GetType().Name}.");
    }
}
