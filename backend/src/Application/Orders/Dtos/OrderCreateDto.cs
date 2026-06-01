namespace ClientAcquisition.Application.Orders.Dtos;

public sealed record OrderCreateDto(
    Guid CustomerId,
    DateTime OrderDate,
    IReadOnlyList<OrderItemCreateDto> Items);