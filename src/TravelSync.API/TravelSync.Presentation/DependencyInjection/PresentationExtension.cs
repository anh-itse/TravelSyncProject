using Microsoft.Extensions.DependencyInjection;

namespace TravelSync.Presentation.DependencyInjection;

public static class PresentationExtension
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        return services;
    }
}
