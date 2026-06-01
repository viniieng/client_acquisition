namespace ClientAcquisition.Application.Orders.Dtos;

public sealed record OrderItemCreateDto(
    string ProductName,
    int Quantity,
    decimal UnitPrice);