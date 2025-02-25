namespace TravelSync.Domain.Exeptions.TravelSync;

public sealed class ProductExeptions
{
    public class ProductNotFoundException(Guid productId)
        : NotFoundException($"The product with the id {productId} was not found.") { }
}
