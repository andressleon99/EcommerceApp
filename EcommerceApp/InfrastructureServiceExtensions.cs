using EcommerceApp.Application.UseCases.Auth;
using EcommerceApp.Application.UseCases.Orders;
using EcommerceApp.Application.UseCases.Products;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Infrastructure.Persistence;
using EcommerceApp.Infrastructure.Repositories;
using EcommerceApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApp.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext con SQLite
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Repositorios
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Servicios
        services.AddScoped<ITokenService, JwtTokenService>();

        // Use Cases
        services.AddScoped<GetAllProductsUseCase>();
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<UpdateProductUseCase>();
        services.AddScoped<DeleteProductUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUseCase>();
        services.AddScoped<CreateOrderUseCase>();
        services.AddScoped<GetUserOrdersUseCase>();

        return services;
    }
}