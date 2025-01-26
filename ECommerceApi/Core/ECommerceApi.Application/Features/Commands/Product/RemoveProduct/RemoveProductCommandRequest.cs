using MediatR;

namespace ECommerceApi.Application.Features.Commands.Product.RemoveProduct;

public class RemoveProductCommandRequest : IRequest<RemoveProductCommandResponse>
{
    public required string Id { get; set; }
}