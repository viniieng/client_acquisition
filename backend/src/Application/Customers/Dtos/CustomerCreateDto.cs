namespace ClientAcquisition.Application.Customers.Dtos;

public sealed record CustomerCreateDto(
    string FullName,
    string Email,
    string Cpf,
    DateOnly BirthDate,
    string Address);