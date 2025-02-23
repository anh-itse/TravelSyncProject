using Microsoft.Extensions.DependencyInjection;
using TravelSync.Domain.Abstractions;
using TravelSync.Domain.Abstractions.Repositories;
using TravelSync.Persistence.Repositories;

namespace TravelSync.Persistence.DependencyInjection.Extensions;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        // Register all scoped dependencies
        var scopedTypes = AssemblyReference.Assembly.ExportedTypes
            .Where(t => typeof(IScopedDependency).IsAssignableFrom(t) && t.IsClass);

        foreach (var scopedType in scopedTypes)
        {
            var interfaceType = scopedType.GetInterfaces().FirstOrDefault(i => i.Name == $"I{scopedType.Name}");

            if (interfaceType != null)  services.AddScoped(interfaceType, scopedType);
        }

        return services;
    }
}
