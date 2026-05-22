using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _ctx;
    public OrderRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _ctx.Orders
            .AsNoTracking()
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .ToListAsync(ct);

    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await _ctx.Orders.AddAsync(order, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _ctx.Orders.Update(order);
        await _ctx.SaveChangesAsync(ct);
    }
}