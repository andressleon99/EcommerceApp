using EcommerceApp.Application.DTOs;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.UseCases.Orders;

public class GetUserOrdersUseCase
{
    private readonly IOrderRepository _orderRepo;

    public GetUserOrdersUseCase(IOrderRepository orderRepo) => _orderRepo = orderRepo;

    public async Task<IEnumerable<OrderResponseDto>> Execute(Guid userId, CancellationToken ct = default)
    {
        var orders = await _orderRepo.GetByUserIdAsync(userId, ct);
        return orders.Select(o => new OrderResponseDto(
            o.Id, o.CreatedAt, o.Status.ToString(), o.Total,
            o.Items.Select(i => new OrderItemResponseDto(
                i.ProductId,
                i.Product?.Name ?? "Producto",
                i.UnitPrice,
                i.Quantity,
                i.Subtotal)).ToList()));
    }
}