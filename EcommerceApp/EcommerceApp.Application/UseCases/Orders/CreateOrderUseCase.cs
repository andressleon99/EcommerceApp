using EcommerceApp.Application.DTOs;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.UseCases.Orders;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;

    public CreateOrderUseCase(IOrderRepository orderRepo, IProductRepository productRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
    }

    public async Task<OrderResponseDto> Execute(Guid userId, CreateOrderRequest request, CancellationToken ct = default)
    {
        var order = Order.Create(userId);

        foreach (var item in request.Items)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId, ct)
                ?? throw new NotFoundException("Producto", item.ProductId);

            order.AddItem(product, item.Quantity);
            await _productRepo.UpdateAsync(product, ct); // persiste el stock descontado
        }

        await _orderRepo.AddAsync(order, ct);

        return MapToDto(order);
    }

    private static OrderResponseDto MapToDto(Order order) =>
        new(order.Id, order.CreatedAt, order.Status.ToString(), order.Total,
            order.Items.Select(i => new OrderItemResponseDto(
                i.ProductId,
                i.Product?.Name ?? "Producto",
                i.UnitPrice,
                i.Quantity,
                i.Subtotal)).ToList());
}