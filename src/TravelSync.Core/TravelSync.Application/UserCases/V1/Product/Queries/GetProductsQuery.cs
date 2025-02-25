using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Domain.DTOs.Products;

namespace TravelSync.Application.UserCases.V1.Product.Queries;

public record GetProductsQuery : IQuery<List<ProductDto>>
{
}

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> HandleAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        return [];
    }
}
