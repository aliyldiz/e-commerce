using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Repositories;
using MediatR;

namespace ECommerceApi.Application.Features.Queries.Order.GetByIdOrder;

public class GetByIdOrderQueryHandler : IRequestHandler<GetByIdOrderQueryRequest, GetByIdOrderQueryResponse>
{
    private readonly IOrderService _service;

    public GetByIdOrderQueryHandler(IOrderService service)
    {
        _service = service;
    }

    public async Task<GetByIdOrderQueryResponse> Handle(GetByIdOrderQueryRequest request, CancellationToken cancellationToken)
    {
        var data = await _service.GetByIdOrderAsync(request.Id);

        return new()
        {
            Id = data.Id,
            OrderCode = data.OrderCode,
            Address = data.Address,
            BasketItems = data.BasketItems,
            CreatedDate = data.CreatedDate,
            Description = data.Description,
        };
    }
}