using System.Security.Claims;
using EcommerceApp.Application.DTOs;
using EcommerceApp.Application.UseCases.Orders;
using EcommerceApp.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Todos los endpoints requieren autenticación
public class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrder;
    private readonly GetUserOrdersUseCase _getUserOrders;

    public OrdersController(CreateOrderUseCase createOrder, GetUserOrdersUseCase getUserOrders)
    {
        _createOrder = createOrder;
        _getUserOrders = getUserOrders;
    }

    // GET api/orders/my — pedidos del usuario autenticado
    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var orders = await _getUserOrders.Execute(userId, ct);
        return Ok(orders);
    }

    // POST api/orders — crear pedido
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var order = await _createOrder.Execute(userId, request, ct);
            return CreatedAtAction(nameof(GetMyOrders), new { id = order.Id }, order);
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
}