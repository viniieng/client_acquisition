using ClientAcquisition.Application.Customers.Dtos;

namespace ClientAcquisition.Application.Common.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponseDto> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default);
    Task<CustomerResponseDto?> UpdateAsync(Guid id, CustomerUpdateDto dto, CancellationToken cancellationToken = default);
    Task<CustomerResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CustomerResponseDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}