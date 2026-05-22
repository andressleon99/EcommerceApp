using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _ctx;
    public ProductRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Products.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Products.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<Product>> GetActiveAsync(CancellationToken ct = default)
        => await _ctx.Products.AsNoTracking().Where(p => p.IsActive).ToListAsync(ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await _ctx.Products.AddAsync(product, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _ctx.Products.Update(product);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Product product, CancellationToken ct = default)
    {
        _ctx.Products.Remove(product);
        await _ctx.SaveChangesAsync(ct);
    }
}