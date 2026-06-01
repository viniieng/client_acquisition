using ClientAcquisition.Domain.Common;

namespace ClientAcquisition.Domain.Entities;

public sealed class Order : Entity
{
    private readonly List<OrderItem> _items = new();

    private Order()
    {
    }

    public Order(Guid customerId, DateTime orderDate, IEnumerable<OrderItem> items)
    {
        CustomerId = customerId;
        OrderDate = orderDate;
        CreatedAt = DateTime.UtcNow;
        ReplaceItems(items);
    }

    public Guid CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Customer? Customer { get; private set; }

    public void Update(DateTime orderDate, IEnumerable<OrderItem> items)
    {
        EnsureEditable();
        OrderDate = orderDate;
        ReplaceItems(items);
    }

    public void ReplaceItems(IEnumerable<OrderItem> items)
    {
        var orderItems = items.ToList();
        if (orderItems.Count == 0)
        {
            throw new InvalidOperationException("Order must have at least one item.");
        }

        _items.Clear();
        foreach (var item in orderItems)
        {
            item.AttachToOrder(Id);
            item.RecalculateSubtotal();
            _items.Add(item);
        }

        RecalculateTotal();
    }

    public void EnsureEditable()
    {
        if (DateTime.UtcNow - CreatedAt > TimeSpan.FromHours(24))
        {
            throw new InvalidOperationException("Order cannot be changed after 24 hours.");
        }
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(item => item.Subtotal);
    }
}