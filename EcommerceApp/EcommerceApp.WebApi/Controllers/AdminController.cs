using EcommerceApp.Application.DTOs;
using EcommerceApp.Application.UseCases.Admin;
using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]  // Solo Admins pueden acceder
public class AdminController : ControllerBase
{
    private readonly AssignPermissionUseCase _assignPermission;
    private readonly IPermissionRepository _permissionRepo;
    private readonly IUserRepository _userRepo;

    public AdminController(
        AssignPermissionUseCase assignPermission,
        IPermissionRepository permissionRepo,
        IUserRepository userRepo)
    {
        _assignPermission = assignPermission;
        _permissionRepo = permissionRepo;
        _userRepo = userRepo;
    }

    // 🆕 Asignar permiso a usuario
    [HttpPost("users/{userId}/permissions")]
    public async Task<IActionResult> AssignPermission(Guid userId, [FromBody] AssignPermissionRequest request, CancellationToken ct)
    {
        try
        {
            await _assignPermission.Execute(userId, request.PermissionId, ct);
            return Ok(new { message = "Permiso asignado exitosamente." });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 🆕 Obtener todos los permisos
    [HttpGet("permissions")]
    public async Task<IActionResult> GetAllPermissions(CancellationToken ct)
    {
        try
        {
            var permissions = await _permissionRepo.GetAllAsync(ct);
            var result = permissions.Select(p => new PermissionDto(p.Id, p.Name)).ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // 🆕 Obtener permisos de un usuario
    [HttpGet("users/{userId}/permissions")]
    public async Task<IActionResult> GetUserPermissions(Guid userId, CancellationToken ct)
    {
        try
        {
            var user = await _userRepo.GetByIdAsync(userId, ct);
            if (user is null)
                return NotFound(new { message = "Usuario no encontrado." });

            var permissions = await _permissionRepo.GetByUserIdAsync(userId, ct);
            var result = new UserPermissionsResponse(
                userId,
                user.Email,
                permissions.Select(p => new PermissionDto(p.Id, p.Name)).ToList()
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
