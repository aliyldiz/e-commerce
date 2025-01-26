using ECommerceApi.Application.Repositories;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.Product.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = await _productRepository.GetByIdAsync(request.Id, false);
        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        await _productRepository.SaveChangesAsync();
        return new();
    }
}