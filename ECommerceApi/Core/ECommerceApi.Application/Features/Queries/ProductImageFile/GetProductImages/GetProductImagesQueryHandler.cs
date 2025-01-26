using ECommerceApi.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerceApi.Application.Features.Queries.ProductImageFile.GetProductImages;

public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IConfiguration _configuration;

    public GetProductImagesQueryHandler(IProductRepository productRepository, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _configuration = configuration;
    }

    public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product? product = await _productRepository.dbSet.Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id!));

        return (product?.ProductImageFiles!).Select(p => new GetProductImagesQueryResponse
        {
            Id = p.Id,
            FileName = p.FileName,
            Path = $"{_configuration["BaseUrlStorage"]}/{p.Path}",
        }).ToList()!;
    }
}