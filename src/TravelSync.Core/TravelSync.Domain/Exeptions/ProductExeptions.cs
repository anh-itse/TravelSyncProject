namespace TravelSync.Domain.Exeptions;

public static class ProductExeptions
{
    public class ProductNotFoundException(Guid productId) : NotFoundException($"The product with the id {productId} was not found.") { }
}
