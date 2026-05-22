using EcommerceApp.Domain.Exceptions;

namespace EcommerceApp.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";

    // Navegación
    public ICollection<Order> Orders { get; private set; } = new List<Order>();

    private User() { }

    public static User Create(string name, string email, string passwordHash, string role = "User")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("El email es inválido.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("La contraseña es obligatoria.");

        return new User
        {
            Name = name.Trim(),
            Email = email.Trim().ToLower(),
            PasswordHash = passwordHash,
            Role = role
        };
    }
}