namespace ClientAcquisition.Application.Customers.Dtos;

public sealed record CustomerResponseDto(
    Guid Id,
    string FullName,
    string Email,
    string Cpf,
    DateOnly BirthDate,
    string Address,
    DateTime CreatedAt);