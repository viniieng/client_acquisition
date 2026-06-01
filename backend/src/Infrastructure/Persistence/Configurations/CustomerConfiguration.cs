using ClientAcquisition.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientAcquisition.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.FullName).IsRequired().HasMaxLength(150);
        builder.Property(customer => customer.Email).IsRequired().HasMaxLength(150);
        builder.Property(customer => customer.Cpf).IsRequired().HasMaxLength(14);
        builder.Property(customer => customer.BirthDate).IsRequired().HasColumnType("date");
        builder.Property(customer => customer.Address).IsRequired().HasMaxLength(250);
        builder.Property(customer => customer.CreatedAt).IsRequired();

        builder.HasIndex(customer => customer.Cpf).IsUnique();

        builder.HasMany(customer => customer.Orders)
            .WithOne(order => order.Customer)
            .HasForeignKey(order => order.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}