using ECommerceApi.Application.Repositories;
using MediatR;

namespace ECommerceApi.Application.Features.Queries.Product.GetByIdProduct;

public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
{
    private readonly IProductRepository _productRepository;

    public GetByIdProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = await _productRepository.GetByIdAsync(request.Id);
        return new()
        {
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
        };
    }
}