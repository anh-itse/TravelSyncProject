using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Application.Decorators;
using TravelSync.Domain.Abstractions.Events;

namespace TravelSync.Application.DependencyInjection.Extensions;

public static class DecoratorRegistration
{
    public static void RegisterDecorators(this IServiceCollection services)
    {
        var handlerInterfaces = new[]
        {
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>),
            typeof(IDomainEventHandler<>),
        };

        var handlersWithAttributes = AssemblyReference.Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition())))
            .Where(t => t.GetCustomAttributes<DecoratorAttribute>().Any())
            .ToList();

        foreach (var handler in handlersWithAttributes)
        {
            var interfaceType = handler.GetInterfaces()
                .First(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition()));

            var decoratorAttributes = handler.GetCustomAttributes<DecoratorAttribute>().ToList();

            services.AddTransient(interfaceType, provider =>
            {
                var pipeline = CreatePipeline(provider, handler, interfaceType, decoratorAttributes);
                return pipeline;
            });
        }
    }

    private static object CreatePipeline(IServiceProvider provider, Type handler, Type interfaceType, List<DecoratorAttribute> decoratorAttributes)
    {
        object currentHandler = ActivatorUtilities.CreateInstance(provider, handler);

        foreach (var attribute in decoratorAttributes)
        {
            var decoratorType = attribute.GetDecoratorType(interfaceType);
            var closedDecoratorType = decoratorType.MakeGenericType(interfaceType.GenericTypeArguments);

            // Lấy constructor đầu tiên của decorator
            var constructor = closedDecoratorType.GetConstructors().FirstOrDefault();
            if (constructor is null) continue;

            // Tự động lấy tất cả dependency parameters
            var parameters = constructor.GetParameters()
                .Select(param =>
                {
                    // Inject handler gốc
                    if (param.ParameterType == interfaceType) return currentHandler;

                    // Nếu là Attribute, kiểm tra có thể lấy từ class decorator không
                    if (param.ParameterType.IsSubclassOf(typeof(Attribute)))
                    {
                        var attr = decoratorAttributes.FirstOrDefault(attr => attr.GetType() == param.ParameterType);
                        if (attr is not null) return attr;
                    }

                    // Nếu không có trong DI, tạo instance thủ công nếu có constructor không tham số
                    var service = provider.GetService(param.ParameterType);
                    if (service is not null) return service;

                    var defaultConstructor = param.ParameterType.GetConstructor(Type.EmptyTypes);
                    if (defaultConstructor is not null) return Activator.CreateInstance(param.ParameterType);

                    // Nếu không thể resolve, bỏ qua
                    return null;
                })
                .ToArray();

            // Nếu có parameter nào không thể resolve, bỏ qua
            if (parameters.Any(p => p is null)) continue;

            // Tạo instance của decorator với danh sách dependency
            currentHandler = ActivatorUtilities.CreateInstance(provider, closedDecoratorType, parameters!);
        }

        return currentHandler;
    }
}
