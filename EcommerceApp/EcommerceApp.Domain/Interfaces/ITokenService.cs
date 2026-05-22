using EcommerceApp.Domain.Entities;

namespace EcommerceApp.Domain.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}