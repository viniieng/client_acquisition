using ClientAcquisition.Application.Customers.Dtos;
using ClientAcquisition.Domain.Services;
using FluentValidation;

namespace ClientAcquisition.Application.Customers.Validators;

public sealed class CustomerCreateDtoValidator : AbstractValidator<CustomerCreateDto>
{
    public CustomerCreateDtoValidator()
    {
        RuleFor(customer => customer.FullName).NotEmpty().MaximumLength(150);
        RuleFor(customer => customer.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(customer => customer.Cpf).NotEmpty().MaximumLength(14).Must(CpfValidator.IsValid).WithMessage("CPF inválido.");
        RuleFor(customer => customer.BirthDate).Must(birthDate => AgeValidator.IsAtLeast18YearsOld(birthDate)).WithMessage("Customer must be at least 18 years old.");
        RuleFor(customer => customer.Address).NotEmpty().MaximumLength(250);
    }
}