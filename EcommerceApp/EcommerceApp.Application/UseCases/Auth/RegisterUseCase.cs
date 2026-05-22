using EcommerceApp.Application.DTOs;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.UseCases.Auth;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;

    public RegisterUseCase(IUserRepository userRepo, ITokenService tokenService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Execute(RegisterRequest request, CancellationToken ct = default)
    {
        var existing = await _userRepo.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            throw new DomainException("Ya existe un usuario con ese email.");

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.Create(request.Name, request.Email, hash);

        await _userRepo.AddAsync(user, ct);

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(token, user.Name, user.Email, user.Role);
    }
}