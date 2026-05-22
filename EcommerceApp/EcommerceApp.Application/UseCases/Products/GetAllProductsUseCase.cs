using EcommerceApp.Application.DTOs;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.UseCases.Products;

public class GetAllProductsUseCase
{
    private readonly IProductRepository _repo;

    public GetAllProductsUseCase(IProductRepository repo) => _repo = repo;

    public async Task<IEnumerable<ProductResponseDto>> Execute(CancellationToken ct = default)
    {
        var products = await _repo.GetActiveAsync(ct);
        return products.Select(p => new ProductResponseDto(p.Id, p.Name, p.Description, p.Price, p.Stock, p.IsActive));
    }
}