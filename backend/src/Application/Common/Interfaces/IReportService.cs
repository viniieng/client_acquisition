using ClientAcquisition.Application.Reports.Dtos;

namespace ClientAcquisition.Application.Common.Interfaces;

public interface IReportService
{
    Task<IReadOnlyList<CustomerSpendingDto>> GetCustomerSpendingAsync(CancellationToken cancellationToken = default);
    Task<CustomerSpendingDto?> GetCustomerSpendingAsync(Guid customerId, CancellationToken cancellationToken = default);
}