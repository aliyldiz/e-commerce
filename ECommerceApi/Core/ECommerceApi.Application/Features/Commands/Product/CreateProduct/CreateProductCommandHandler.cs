using ECommerceApi.Application.Abstractions.Hubs;
using ECommerceApi.Application.Repositories;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.Product.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductHubService _productHubService;

    public CreateProductCommandHandler(IProductRepository productRepository, IProductHubService productHubService)
    {
        _productRepository = productRepository;
        _productHubService = productHubService;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        await _productRepository.AddAsync(new Domain.Entities.Product()
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        });
        await _productRepository.SaveChangesAsync();
        await _productHubService.ProductAddedMessageAsync($"Product added. \n Name: {request.Name}, Price: {request.Price}, Stock: {request.Stock}");
        return new();
    }
}