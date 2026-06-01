using System.Reflection;
using ClientAcquisition.Domain.Entities;

namespace ClientAcquisition.Tests;

public sealed class OrderRulesTests
{
    [Fact]
    public void Order_CalculatesTotalAmount_FromItems()
    {
        var order = new Order(Guid.NewGuid(), DateTime.UtcNow, new[]
        {
            new OrderItem("Keyboard", 2, 150m),
            new OrderItem("Mouse", 1, 50m)
        });

        Assert.Equal(350m, order.TotalAmount);
    }

    [Fact]
    public void EnsureEditable_Throws_WhenOrderIsOlderThan24Hours()
    {
        var order = new Order(Guid.NewGuid(), DateTime.UtcNow, new[]
        {
            new OrderItem("Keyboard", 1, 150m)
        });

        var createdAtProperty = typeof(Order).GetProperty("CreatedAt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        createdAtProperty!.SetValue(order, DateTime.UtcNow.AddHours(-25));

        var exception = Assert.Throws<InvalidOperationException>(() => order.EnsureEditable());

        Assert.Equal("Order cannot be changed after 24 hours.", exception.Message);
    }
}