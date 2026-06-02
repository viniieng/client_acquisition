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
    public void EnsureEditable_Throws_WhenOrderIsYoungerThan24Hours()
    {
        var order = new Order(Guid.NewGuid(), DateTime.UtcNow, new[]
        {
            new OrderItem("Keyboard", 1, 150m)
        });

        var exception = Assert.Throws<InvalidOperationException>(() => order.EnsureEditable());

        Assert.Equal(Order.EditLockMessage, exception.Message);
    }

    [Fact]
    public void IsEditable_ReturnsFalse_WhenOrderIsYoungerThan24Hours()
    {
        var order = new Order(Guid.NewGuid(), DateTime.UtcNow, new[]
        {
            new OrderItem("Keyboard", 1, 150m)
        });

        Assert.False(order.IsEditable());
    }

    [Fact]
    public void IsEditable_ReturnsTrue_WhenOrderIsOlderThan24Hours()
    {
        var order = new Order(Guid.NewGuid(), DateTime.UtcNow, new[]
        {
            new OrderItem("Keyboard", 1, 150m)
        });

        var createdAtProperty = typeof(Order).GetProperty("CreatedAt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        createdAtProperty!.SetValue(order, DateTime.UtcNow.AddHours(-25));

        Assert.True(order.IsEditable());
    }

    [Fact]
    public void EnsureEditable_DoesNotThrow_WhenOrderIsOlderThan24Hours()
    {
        var order = new Order(Guid.NewGuid(), DateTime.UtcNow, new[]
        {
            new OrderItem("Keyboard", 1, 150m)
        });

        var createdAtProperty = typeof(Order).GetProperty("CreatedAt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        createdAtProperty!.SetValue(order, DateTime.UtcNow.AddHours(-25));

        order.EnsureEditable();
    }
}