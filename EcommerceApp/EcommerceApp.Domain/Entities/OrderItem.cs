namespace EcommerceApp.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem() { }

    public static OrderItem Create(Guid orderId, Guid productId, decimal unitPrice, int quantity)
    {
        return new OrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            UnitPrice = unitPrice,
            Quantity = quantity
        };
    }

    public void IncreaseQuantity(int extra) => Quantity += extra;
}