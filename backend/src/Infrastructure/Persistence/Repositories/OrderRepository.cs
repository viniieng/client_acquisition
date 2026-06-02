using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientAcquisition.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Orders
            .Include(order => order.Customer)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(order => order.Customer)
            .Include(order => order.Items)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetPagedAsync(int page, int pageSize, string? customerName, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default)
    {
        var query = _context.Orders
            .Include(order => order.Customer)
            .Include(order => order.Items)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            query = query.Where(order => order.Customer != null && order.Customer.FullName.Contains(customerName));
        }

        if (startDate.HasValue)
        {
            query = query.Where(order => order.OrderDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(order => order.OrderDate <= endDate.Value);
        }

        return await query
            .OrderByDescending(order => order.OrderDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        return _context.Orders.AddAsync(order, cancellationToken).AsTask();
    }

    public async Task ReplaceItemsAsync(Order order, CancellationToken cancellationToken = default)
    {
        var existingItems = await _context.OrderItems
            .Where(item => item.OrderId == order.Id)
            .ToListAsync(cancellationToken);

        if (existingItems.Count > 0)
        {
            _context.OrderItems.RemoveRange(existingItems);
        }

        foreach (var item in order.Items)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                await _context.OrderItems.AddAsync(item, cancellationToken);
            }
        }
    }

    public void Delete(Order order)
    {
        _context.Orders.Remove(order);
    }
}