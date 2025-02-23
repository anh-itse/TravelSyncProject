using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Domain.Abstractions.Events;

namespace TravelSync.Application.DependencyInjection.Extensions;

public static class DispatchingRegistration
{
    /// <summary>
    /// Registers handler interfaces and their implementations in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the handlers to.</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided assembly is null.</exception>
    public static void RegistrationHandlerInterfaces(this IServiceCollection services)
    {
        Assembly assembly = AssemblyReference.Assembly;

        // Define the set of handler interfaces to look for.
        var handlerInterfaces = new HashSet<Type>
        {
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>),
            typeof(IDomainEventHandler<>),
        };

        // Get all types in the assembly that are classes and not abstract.
        var handlerTypes = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && !IsDecorator(type))
            .Select(type => new
            {
                Implementation = type,
                Interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition()))
                    .ToList(),
            })
            .Where(x => x.Interfaces.Count > 0)
            .ToList();

        // Register each handler implementation with its corresponding interface.
        foreach (var handler in handlerTypes)
        {
            foreach (var interfaceType in handler.Interfaces)
            {
                services.AddTransient(interfaceType, handler.Implementation);
            }
        }
    }

    private static bool IsDecorator(Type type)
    {
        var handlerInterfaces = new HashSet<Type>
    {
        typeof(ICommandHandler<>),
        typeof(ICommandHandler<,>),
        typeof(IQueryHandler<,>),
        typeof(IDomainEventHandler<>),
    };

        return type.GetConstructors()
            .Any(ctor => ctor.GetParameters()
                .Any(param => param.ParameterType.IsGenericType
                            && handlerInterfaces.Contains(param.ParameterType.GetGenericTypeDefinition())));
    }
}
