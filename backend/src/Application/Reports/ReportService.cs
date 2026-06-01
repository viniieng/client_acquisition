using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Application.Reports.Dtos;

namespace ClientAcquisition.Application.Reports;

public sealed class ReportService : IReportService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;

    public ReportService(ICustomerRepository customerRepository, IOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<CustomerSpendingDto>> GetCustomerSpendingAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        var customers = new List<CustomerSpendingDto>();

        foreach (var group in orders.GroupBy(order => order.CustomerId))
        {
            var customer = await _customerRepository.GetByIdAsync(group.Key, cancellationToken);
            if (customer is null)
            {
                continue;
            }

            customers.Add(new CustomerSpendingDto(customer.Id, customer.FullName, group.Count(), group.Sum(order => order.TotalAmount)));
        }

        return customers;
    }

    public async Task<CustomerSpendingDto?> GetCustomerSpendingAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        var customerOrders = orders.Where(order => order.CustomerId == customerId).ToList();

        return new CustomerSpendingDto(customer.Id, customer.FullName, customerOrders.Count, customerOrders.Sum(order => order.TotalAmount));
    }
}