using ClientAcquisition.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientAcquisition.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(item => item.Id);

        builder.Property(item => item.OrderId).IsRequired();
        builder.Property(item => item.ProductName).IsRequired().HasMaxLength(150);
        builder.Property(item => item.Quantity).IsRequired();
        builder.Property(item => item.UnitPrice).IsRequired().HasPrecision(18, 2);
        builder.Property(item => item.Subtotal).IsRequired().HasPrecision(18, 2);
    }
}