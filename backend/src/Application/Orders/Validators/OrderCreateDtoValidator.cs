using ClientAcquisition.Application.Orders.Dtos;
using FluentValidation;

namespace ClientAcquisition.Application.Orders.Validators;

public sealed class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
{
    public OrderCreateDtoValidator()
    {
        RuleFor(order => order.CustomerId).NotEmpty();
        RuleFor(order => order.OrderDate).NotEmpty();
        RuleFor(order => order.Items).NotEmpty().WithMessage("Order must have at least one item.");
        RuleForEach(order => order.Items).SetValidator(new OrderItemCreateDtoValidator());
    }
}