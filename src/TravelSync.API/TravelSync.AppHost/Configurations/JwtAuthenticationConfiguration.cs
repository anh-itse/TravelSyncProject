using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TravelSync.AppHost.Configurations;

public static class JwtAuthenticationConfiguration
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Jwt:Authority"];
                options.Audience = configuration["Jwt:Audience"];
            });

        return services;
    }
}
