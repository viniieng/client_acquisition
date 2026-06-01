using ClientAcquisition.Application.Orders.Dtos;
using FluentValidation;

namespace ClientAcquisition.Application.Orders.Validators;

public sealed class OrderItemCreateDtoValidator : AbstractValidator<OrderItemCreateDto>
{
    public OrderItemCreateDtoValidator()
    {
        RuleFor(item => item.ProductName).NotEmpty().MaximumLength(150);
        RuleFor(item => item.Quantity).GreaterThan(0);
        RuleFor(item => item.UnitPrice).GreaterThan(0);
    }
}