using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.DTOs.Order;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.Order.CompleteOrder;

public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
{
    private readonly IOrderService _service;
    private readonly IMailService _mailService;

    public CompleteOrderCommandHandler(IOrderService service, IMailService mailService)
    {
        _service = service;
        _mailService = mailService;
    }

    public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
    {
        (bool succeded, CompletedOrderDTO dto) = await _service.CompleteOrderAsync(request.Id);
        if (succeded)
            await _mailService.SendCompletedOrderMailAsync(dto.EMail, dto.OrderCode, dto.OrderDate, dto.Username);
        return new();
    }
}