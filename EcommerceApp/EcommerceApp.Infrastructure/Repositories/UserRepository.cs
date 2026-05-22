using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;
    public UserRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Users.FindAsync(new object[] { id }, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _ctx.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email.ToLower(), ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await _ctx.Users.AddAsync(user, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}