using AutoMapper;
using ClientAcquisition.Application.Common.Exceptions;
using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Application.Orders.Dtos;
using ClientAcquisition.Domain.Entities;

namespace ClientAcquisition.Application.Orders;

public sealed class OrderService : IOrderService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(ICustomerRepository customerRepository, IOrderRepository orderRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponseDto> CreateAsync(OrderCreateDto dto, CancellationToken cancellationToken = default)
    {
        EnsureBusinessRules(dto);

        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, cancellationToken);
        if (customer is null)
        {
            throw new BusinessRuleException("Customer not found.");
        }

        var items = dto.Items.Select(item => new OrderItem(item.ProductName, item.Quantity, item.UnitPrice)).ToList();
        var order = new Domain.Entities.Order(dto.CustomerId, dto.OrderDate, items);

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapOrder(order, customer.FullName);
    }

    public async Task<OrderResponseDto?> UpdateAsync(Guid id, OrderUpdateDto dto, CancellationToken cancellationToken = default)
    {
        EnsureBusinessRules(dto);

        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return null;
        }

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId, cancellationToken);
        if (customer is null)
        {
            throw new BusinessRuleException("Customer not found.");
        }

        var items = dto.Items.Select(item => new OrderItem(item.ProductName, item.Quantity, item.UnitPrice)).ToList();
        order.Update(dto.OrderDate, items);
        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapOrder(order, customer.FullName);
    }

    public async Task<OrderResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return null;
        }

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId, cancellationToken);
        return MapOrder(order, customer?.FullName ?? string.Empty);
    }

    public async Task<IReadOnlyList<OrderResponseDto>> GetPagedAsync(int page, int pageSize, string? customerName, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetPagedAsync(page, pageSize, customerName, startDate, endDate, cancellationToken);
        var result = new List<OrderResponseDto>();

        foreach (var order in orders)
        {
            var customer = await _customerRepository.GetByIdAsync(order.CustomerId, cancellationToken);
            result.Add(MapOrder(order, customer?.FullName ?? string.Empty));
        }

        return result;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return;
        }

        order.EnsureEditable();
        _orderRepository.Delete(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureBusinessRules(OrderCreateDto dto)
    {
        if (dto.Items is null || dto.Items.Count == 0)
        {
            throw new BusinessRuleException("Order must have at least one item.");
        }
    }

    private static void EnsureBusinessRules(OrderUpdateDto dto)
    {
        if (dto.Items is null || dto.Items.Count == 0)
        {
            throw new BusinessRuleException("Order must have at least one item.");
        }
    }

    private OrderResponseDto MapOrder(Domain.Entities.Order order, string customerFullName)
    {
        var itemDtos = order.Items.Select(item => new OrderItemResponseDto(item.Id, item.OrderId, item.ProductName, item.Quantity, item.UnitPrice, item.Subtotal)).ToList();
        return new OrderResponseDto(order.Id, order.CustomerId, customerFullName, order.OrderDate, order.TotalAmount, order.CreatedAt, itemDtos);
    }
}