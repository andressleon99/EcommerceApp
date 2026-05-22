using EcommerceApp.Application.DTOs;
using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.UseCases.Products;

public class UpdateProductUseCase
{
    private readonly IProductRepository _repo;

    public UpdateProductUseCase(IProductRepository repo) => _repo = repo;

    public async Task Execute(Guid id, UpdateProductDto dto, CancellationToken ct = default)
    {
        var product = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Producto", id);

        product.Update(dto.Name, dto.Description, dto.Price);
        await _repo.UpdateAsync(product, ct);
    }
}