using System.ComponentModel.DataAnnotations;

namespace ClientAcquisition.Frontend.Models.Forms;

public sealed class CustomerForm
{
    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome completo deve ter entre 3 e 150 caracteres.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    public DateOnly? BirthDate { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [StringLength(250, ErrorMessage = "O endereço deve ter no máximo 250 caracteres.")]
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
