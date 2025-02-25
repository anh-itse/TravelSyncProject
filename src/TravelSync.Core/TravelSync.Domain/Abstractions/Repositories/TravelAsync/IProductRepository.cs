using TravelSync.Domain.DTOs.Products;
using TravelSync.Domain.Entities;

namespace TravelSync.Domain.Abstractions.Repositories.TravelAsync;

public interface IProductRepository : IRepository<Product, Guid>, IScopedDependency
{
    Task CreateProductAsync(ProductDto product, CancellationToken cancellationToken = default);
}