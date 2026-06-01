namespace ClientAcquisition.Application.Orders.Dtos;

public sealed record OrderResponseDto(
    Guid Id,
    Guid CustomerId,
    string CustomerFullName,
    DateTime OrderDate,
    decimal TotalAmount,
    DateTime CreatedAt,
    IReadOnlyList<OrderItemResponseDto> Items);