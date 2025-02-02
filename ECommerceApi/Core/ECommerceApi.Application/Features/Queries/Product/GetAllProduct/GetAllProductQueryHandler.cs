using ECommerceApi.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Application.Features.Queries.Product.GetAllProduct;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
    {
        var totalProductCount = _productRepository.Get(null, true, null).Count();
        var products = _productRepository.Get(null, true, null)
            .Skip((request.Page) * request.Size).Include(p => p.ProductImageFiles).Take(request.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.ModifiedDate,
                p.ProductImageFiles
            }).ToList();

        return Task.FromResult<GetAllProductQueryResponse>(new()
        {
            Products = products,
            TotalProductCount = totalProductCount
        });
    }
}