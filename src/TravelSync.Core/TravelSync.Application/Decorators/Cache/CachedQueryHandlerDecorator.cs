using Microsoft.Extensions.Logging;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Shared.Helpers;

namespace TravelSync.Application.Decorators.Cache;

public sealed class CachedQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> innerHandler,
    ILogger<CachedQueryHandlerDecorator<TQuery, TResult>> logger
    ) : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        var cacheAttribute = (CacheAttribute?)Attribute.GetCustomAttribute(query.GetType(), typeof(CacheAttribute));

        if (cacheAttribute == null)
        {
            return await innerHandler.HandleAsync(query, cancellationToken);
        }

        string cacheKey = GenerateCacheKey(query, cacheAttribute);
        //var cachedData = await cache.GetAsync<TResult>(cacheKey);

        //if (cachedData != null)
        //{
        //    LogHelper.Info(logger, $"Cache hit: {cacheKey}");
        //    return cachedData;
        //}

        LogHelper.Info(logger, $"Cache miss {typeof(TQuery).Name}: {cacheKey}");
        var result = await innerHandler.HandleAsync(query, cancellationToken);
        //await cache.SetAsync(cacheKey, result, cacheAttribute.Duration);

        return result;
    }

    private static string GenerateCacheKey(TQuery query, CacheAttribute cacheAttribute)
    {
        var queryType = query.GetType().Name;
        var propertyValues = cacheAttribute.KeyProperties
            .Select(prop => $"{prop}={query.GetType().GetProperty(prop)?.GetValue(query)}")
            .ToArray();

        return $"{queryType}:{string.Join(",", propertyValues)}";
    }
}
