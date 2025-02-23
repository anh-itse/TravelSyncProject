using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelSync.Persistence.Interceptors;

namespace TravelSync.Persistence.DependencyInjection.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfigurationSection section)
    {
        services.AddSqlConfiguration();
        services.ConfigureSqlServerRetryOptions(section);

        services.AddScoped<AuditInterceptor>();

        return services;
    }
}
