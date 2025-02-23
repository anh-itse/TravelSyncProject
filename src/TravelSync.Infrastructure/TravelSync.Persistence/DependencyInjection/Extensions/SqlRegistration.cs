using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TravelSync.Persistence.DependencyInjection.Options;
using TravelSync.Persistence.Entities.Identity;

namespace TravelSync.Persistence.DependencyInjection.Extensions;

public static class SqlRegistration
{
    public static IServiceCollection AddSqlConfiguration(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var retryOptions = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

            options.EnableDetailedErrors()
                .UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.ExecutionStrategy(
                        dependencies => new SqlServerRetryingExecutionStrategy(
                            dependencies,
                            retryOptions.CurrentValue.MaxRetryCount,
                            retryOptions.CurrentValue.MaxRetryDelay,
                            retryOptions.CurrentValue.ErrorNumbersToAdd))
                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));
        }, ServiceLifetime.Scoped);

        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            options.EnableDetailedErrors();
        }, ServiceLifetime.Scoped);

        services.AddIdentityCore<AppUser>()
               .AddRoles<AppRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            options.Lockout.MaxFailedAccessAttempts = 3;

            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 1;

            options.User.RequireUniqueEmail = true;
        });

        return services;
    }

    /// <summary>
    /// Kiểm tra và thêm cấu hình RetryOption cho SQL Server.
    /// </summary>
    /// <param name="services">The service collection to which the options will be added.</param>
    /// <param name="section">The configuration section containing the SQL Server retry options.</param>
    /// <returns>The service collection to which the options were added.</returns>
    public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<SqlServerRetryOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}
