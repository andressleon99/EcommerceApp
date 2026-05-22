namespace EcommerceApp.Application.DTOs;

public record CreateOrderRequest(List<OrderItemRequest> Items);
public record OrderItemRequest(Guid ProductId, int Quantity);
public record OrderResponseDto(Guid Id, DateTime CreatedAt, string Status, decimal Total, List<OrderItemResponseDto> Items);
public record OrderItemResponseDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal Subtotal);