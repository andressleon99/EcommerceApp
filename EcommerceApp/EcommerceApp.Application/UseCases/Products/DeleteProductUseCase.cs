using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.UseCases.Products;

public class DeleteProductUseCase
{
    private readonly IProductRepository _repo;

    public DeleteProductUseCase(IProductRepository repo) => _repo = repo;

    public async Task Execute(Guid id, CancellationToken ct = default)
    {
        var product = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Producto", id);

        product.Deactivate();
        await _repo.UpdateAsync(product, ct);
    }
}