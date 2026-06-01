namespace ClientAcquisition.Frontend.Models;

public sealed record OrderItemResponseDto(
    Guid Id,
    Guid OrderId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal);