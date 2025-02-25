using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Application.Decorators.AutoRetry;
using TravelSync.Domain.Abstractions;
using TravelSync.Domain.Abstractions.Repositories.TravelAsync;
using TravelSync.Domain.DTOs.Products;

namespace TravelSync.Application.UserCases.V1.Product.Commands;

public record CreateProductCommand(ProductDto Input) : ICommand
{
}

[AutoRetry]
internal sealed class CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductCommand>
{
    public async Task HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        await using var uow = await unitOfWork.BeginTransactionAsync(cancellationToken);
        await productRepository.CreateProductAsync(command.Input, cancellationToken);
        await productRepository.CreateProductAsync(command.Input, cancellationToken);
        var httpClient = new HttpClient(new FakeHttpMessageHandler());
        await httpClient.GetAsync("https://fakeapi.com");
        await productRepository.CreateProductAsync(command.Input, cancellationToken);
        await productRepository.CreateProductAsync(command.Input, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }
}

public class FakeHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw new HttpRequestException("Fake HTTP request exception for testing");
    }
}
