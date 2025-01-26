using ECommerceApi.Application.Repositories;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.Product.RemoveProduct;

public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
{
    private readonly IProductRepository _productRepository;

    public RemoveProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
    {
        await _productRepository.DeleteAsync(Guid.Parse(request.Id));
        await _productRepository.SaveChangesAsync();
        return new();
    }
}