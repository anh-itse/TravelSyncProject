namespace TravelSync.Domain.Helpers;

public static class DecoratorHelper
{
    public static bool HasAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute
    {
        return type == null
            ? throw new ArgumentNullException(nameof(type))
            : type.GetCustomAttributes(typeof(TAttribute), true).Length != 0;
    }
}
