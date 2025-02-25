using Microsoft.EntityFrameworkCore;
using TravelSync.Domain.Abstractions;
using TravelSync.Domain.Abstractions.Repositories.TravelAsync;
using TravelSync.Domain.DTOs.Products;
using TravelSync.Domain.Entities;

namespace TravelSync.Persistence.Repositories.TravelAsync;

public class ProductRepository(
        ApplicationDbContext appDbContext
    ) : Repository<Product, Guid>(appDbContext), IProductRepository
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
