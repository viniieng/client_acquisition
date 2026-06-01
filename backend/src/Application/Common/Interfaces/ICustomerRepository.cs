using ClientAcquisition.Domain.Entities;

namespace ClientAcquisition.Application.Common.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Customer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    void Update(Customer customer);
    void Delete(Customer customer);
}