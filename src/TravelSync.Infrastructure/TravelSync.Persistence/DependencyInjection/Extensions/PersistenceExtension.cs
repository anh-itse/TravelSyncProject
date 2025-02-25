using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelSync.Domain.Abstractions;
using TravelSync.Persistence.Interceptors;
using TravelSync.Persistence.Repositories;

namespace TravelSync.Persistence.DependencyInjection.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfigurationSection section)
    {
        services.AddSqlConfiguration();
        services.AddRepositories();

        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
