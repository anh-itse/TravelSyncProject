namespace TravelSync.Application.Decorators;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class DecoratorAttribute : Attribute
{
    public abstract Type GetDecoratorType(Type handlerType);
}
