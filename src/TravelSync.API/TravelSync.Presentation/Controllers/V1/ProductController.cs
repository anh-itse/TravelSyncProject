using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Presentation.Abstractions;
using TravelSync.Shared.DTOs.Product;

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

    //[HttpPost]
    //public async Task<IActionResult> CreateProduct(CreateProductCommand command)
    //{
    //    var result = await this.Dispatcher.SendAsync(command);
    //    return Ok(result);
    //}

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
