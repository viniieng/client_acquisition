using System.ComponentModel.DataAnnotations;

namespace ClientAcquisition.Frontend.Models.Forms;

public sealed class CustomerForm
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 150 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF is required.")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birth date is required.")]
    public DateOnly? BirthDate { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(250, ErrorMessage = "Address must be at most 250 characters.")]
    public string Address { get; set; } = string.Empty;

    public static CustomerForm FromCustomer(CustomerResponseDto customer) => new()
    {
        FullName = customer.FullName,
        Email = customer.Email,
        Cpf = customer.Cpf,
        BirthDate = customer.BirthDate,
        Address = customer.Address
    };

    public object ToPayload() => new
    {
        fullName = FullName,
        email = Email,
        cpf = Cpf,
        birthDate = BirthDate,
        address = Address
    };
}
