using ECommerceApi.Application.Repositories;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.Product.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
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
        return new();
    }
}