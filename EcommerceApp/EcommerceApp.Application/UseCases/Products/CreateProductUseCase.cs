using EcommerceApp.Application.DTOs;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.UseCases.Products;

public class CreateProductUseCase
{
    private readonly IProductRepository _repo;

    public CreateProductUseCase(IProductRepository repo) => _repo = repo;

    public async Task<ProductResponseDto> Execute(CreateProductDto dto, CancellationToken ct = default)
    {
        var product = Product.Create(dto.Name, dto.Description, dto.Price, dto.Stock);
        await _repo.AddAsync(product, ct);
        return new ProductResponseDto(product.Id, product.Name, product.Description, product.Price, product.Stock, product.IsActive);
    }
}