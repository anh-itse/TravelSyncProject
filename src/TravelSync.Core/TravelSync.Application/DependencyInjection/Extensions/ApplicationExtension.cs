using Microsoft.Extensions.DependencyInjection;

namespace TravelSync.Application.DependencyInjection.Extensions;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.RegistrationHandlerInterfaces();
        services.RegisterDecorators();
        return services;
    }
}
