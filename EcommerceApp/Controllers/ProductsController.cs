using EcommerceApp.Application.DTOs;
using EcommerceApp.Application.UseCases.Products;
using EcommerceApp.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly GetAllProductsUseCase _getAll;
    private readonly CreateProductUseCase _create;
    private readonly UpdateProductUseCase _update;
    private readonly DeleteProductUseCase _delete;

    public ProductsController(
        GetAllProductsUseCase getAll,
        CreateProductUseCase create,
        UpdateProductUseCase update,
        DeleteProductUseCase delete)
    {
        _getAll = getAll;
        _create = create;
        _update = update;
        _delete = delete;
    }

    // GET api/products — público
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var products = await _getAll.Execute(ct);
        return Ok(products);
    }

    // POST api/products — solo Admin
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken ct)
    {
        try
        {
            var product = await _create.Execute(dto, ct);
            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/products/{id} — solo Admin
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto, CancellationToken ct)
    {
        try
        {
            await _update.Execute(id, dto, ct);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE api/products/{id} — solo Admin
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _delete.Execute(id, ct);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}