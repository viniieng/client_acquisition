using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientAcquisition.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Customers.AsNoTracking().FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    public Task<Customer?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return _context.Customers.AsNoTracking().FirstOrDefaultAsync(customer => customer.Cpf == cpf, cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        return _context.Customers.AddAsync(customer, cancellationToken).AsTask();
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public void Delete(Customer customer)
    {
        _context.Customers.Remove(customer);
    }
}