using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Application.Behaviors;
using TravelSync.Domain.Abstractions.Identity;
using TravelSync.Infrastructure.Dispatching;
using TravelSync.Infrastructure.Identity;

namespace TravelSync.Infrastructure.DependencyInjection.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddScoped<Dispatcher>();

        // Đăng ký PipelineBehavior
        services.AddScoped<IDispatcher>(provider =>
        {
            var innerDispatcher = provider.GetRequiredService<Dispatcher>();
            var validationPipeline = new ValidationPipelineBehavior(innerDispatcher, provider);

            var logger = provider.GetRequiredService<ILogger<ResultHandlingPipelineBehavior>>();
            return new ResultHandlingPipelineBehavior(validationPipeline, logger);
        });

        return services;
    }
}
