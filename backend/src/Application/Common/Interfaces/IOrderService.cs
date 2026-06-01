using ClientAcquisition.Application.Orders.Dtos;

namespace ClientAcquisition.Application.Common.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(OrderCreateDto dto, CancellationToken cancellationToken = default);
    Task<OrderResponseDto?> UpdateAsync(Guid id, OrderUpdateDto dto, CancellationToken cancellationToken = default);
    Task<OrderResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderResponseDto>> GetPagedAsync(int page, int pageSize, string? customerName, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}