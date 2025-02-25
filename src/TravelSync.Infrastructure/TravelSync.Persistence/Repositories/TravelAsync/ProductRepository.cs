using TravelSync.Domain.Abstractions.Repositories.TravelAsync;
using TravelSync.Domain.DTOs.Products;
using TravelSync.Domain.Entities;
using TravelSync.Persistence.Abstractions;

namespace TravelSync.Persistence.Repositories.TravelAsync;

public class ProductRepository(IDbContextProvider dbContextProvider) : Repository<Product, Guid>(dbContextProvider), IProductRepository
{
    public async Task CreateProductAsync(ProductDto product, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(product);

        await this.InsertAsync(new Product
        {
            Id = Guid.NewGuid(),
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
        }, cancellationToken);
    }
}
