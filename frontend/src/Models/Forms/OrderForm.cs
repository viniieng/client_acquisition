using System.ComponentModel.DataAnnotations;

namespace ClientAcquisition.Frontend.Models.Forms;

public sealed class OrderForm
{
    [Required(ErrorMessage = "Selecione um cliente.")]
    public Guid? CustomerId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public List<OrderItemForm> Items { get; set; } = new() { new OrderItemForm() };

    public decimal Total => Items.Sum(item => item.Subtotal);

    public object ToPayload() => new
    {
        customerId = CustomerId,
        orderDate = OrderDate,
        items = Items.Select(item => new
        {
            productName = item.ProductName,
            quantity = item.Quantity,
            unitPrice = item.UnitPrice
        })
    };
}

public sealed class OrderItemForm
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser de pelo menos 1.")]
    public int Quantity { get; set; } = 1;

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que 0.")]
    public decimal UnitPrice { get; set; }

    public decimal Subtotal => Quantity * UnitPrice;
}
