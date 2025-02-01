using ECommerceApi.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Application.Features.Queries.Product.GetAllProduct;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetAllProductQueryHandler> _logger;

    public GetAllProductQueryHandler(IProductRepository productRepository, ILogger<GetAllProductQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("get all products :)");
        
        var totalCount = _productRepository.Get(null, true, null).Count();
        var products = _productRepository.Get(null, true, null)
            .Skip((request.Page) * request.Size).Take(request.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.ModifiedDate,
            }).ToList();

        return Task.FromResult<GetAllProductQueryResponse>(new()
        {
            Products = products,
            TotalCount = totalCount
        });
    }
}