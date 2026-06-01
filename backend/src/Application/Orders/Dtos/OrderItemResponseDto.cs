namespace ClientAcquisition.Application.Orders.Dtos;

public sealed record OrderItemResponseDto(
    Guid Id,
    Guid OrderId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal);