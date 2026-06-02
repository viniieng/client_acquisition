using ClientAcquisition.Domain.Entities;

namespace ClientAcquisition.Application.Common.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetPagedAsync(int page, int pageSize, string? customerName, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task ReplaceItemsAsync(Order order, CancellationToken cancellationToken = default);
    void Delete(Order order);
}