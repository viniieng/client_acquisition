using ClientAcquisition.Domain.Common;

namespace ClientAcquisition.Domain.Entities;

public sealed class OrderItem : Entity
{
    private OrderItem()
    {
    }

    public OrderItem(string productName, int quantity, decimal unitPrice)
    {
        ProductName = productName.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        RecalculateSubtotal();
    }

    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Subtotal { get; private set; }

    public void AttachToOrder(Guid orderId)
    {
        OrderId = orderId;
    }

    public void RecalculateSubtotal()
    {
        Subtotal = Quantity * UnitPrice;
    }
}