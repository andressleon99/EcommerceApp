using EcommerceApp.Domain.Enums;
using EcommerceApp.Domain.Exceptions;

namespace EcommerceApp.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

    public decimal Total => Items.Sum(i => i.Subtotal);

    private Order() { }

    public static Order Create(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("El usuario es obligatorio.");

        return new Order
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };
    }

    public void AddItem(Product product, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La cantidad debe ser mayor a cero.");

        product.RemoveStock(quantity); // valida stock dentro de la entidad

        var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            Items.Add(OrderItem.Create(Id, product.Id, product.Price, quantity));
        }
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Solo se pueden confirmar pedidos pendientes.");
        if (!Items.Any())
            throw new DomainException("El pedido no tiene productos.");
        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new DomainException("No se puede cancelar un pedido entregado.");
        Status = OrderStatus.Cancelled;
    }
}