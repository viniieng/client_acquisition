namespace ClientAcquisition.Application.Customers.Dtos;

public sealed record CustomerUpdateDto(
    string FullName,
    string Email,
    string Cpf,
    DateOnly BirthDate,
    string Address);