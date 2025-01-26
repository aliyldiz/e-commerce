using MediatR;

namespace ECommerceApi.Application.Features.Commands.Product.UpdateProduct;

public class UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
}