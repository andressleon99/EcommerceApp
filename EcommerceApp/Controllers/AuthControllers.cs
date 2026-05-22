using EcommerceApp.Application.DTOs;
using EcommerceApp.Application.UseCases.Auth;
using EcommerceApp.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterUseCase _register;
    private readonly LoginUseCase _login;

    public AuthController(RegisterUseCase register, LoginUseCase login)
    {
        _register = register;
        _login = login;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _register.Execute(request, ct);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _login.Execute(request, ct);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}