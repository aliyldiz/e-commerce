using MediatR;

namespace ECommerceApi.Application.Features.Commands.Product.CreateProduct;

public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
{
    public required string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
}