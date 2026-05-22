using EcommerceApp.Application.DTOs;
using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;

    public LoginUseCase(IUserRepository userRepo, ITokenService tokenService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Execute(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userRepo.GetByEmailAsync(request.Email, ct);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new DomainException("Credenciales incorrectas.");

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(token, user.Name, user.Email, user.Role);
    }
}