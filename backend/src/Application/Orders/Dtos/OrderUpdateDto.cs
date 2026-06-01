namespace ClientAcquisition.Application.Orders.Dtos;

public sealed record OrderUpdateDto(
    DateTime OrderDate,
    IReadOnlyList<OrderItemCreateDto> Items);