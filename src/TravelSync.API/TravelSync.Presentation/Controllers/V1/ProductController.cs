using Microsoft.AspNetCore.Mvc;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Application.UserCases.V1.Product.Commands;
using TravelSync.Domain.DTOs.Products;
using TravelSync.Presentation.Abstractions;

namespace TravelSync.Presentation.Controllers.V1;

public class ProductController(IDispatcher dispatcher) : ApiController(dispatcher)
{
    //[HttpGet(Name = "GetProducts")]
    //[ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    //{
    //    var products = await this.Dispatcher.GetResultAsync(new GetProductsQuery());
    //    return Ok(products);
    //}

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetProduct(int id)
    //{
    //    var product = await this.Dispatcher.GetResultAsync(new GetProductQuery(id));
    //    return Ok(product);
    //}

    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct(ProductDto product, CancellationToken cancellationToken)
    {
        var result = await this.Dispatcher.DispatchAsync(new CreateProductCommand(product), cancellationToken);

        if (result == null || result.IsFailure) return this.BadRequest(result);

        return this.Ok(result);
    }

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommand command)
    //{
    //    command.Id = id;
    //    var result = await this.Dispatcher.SendAsync(command);
    //    return Ok(result);
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteProduct(int id)
    //{
    //    var command = new DeleteProductCommand(id);
    //    var result = await this.Dispatcher.SendAsync(command);
    //    return Ok(result);
    //}
}
