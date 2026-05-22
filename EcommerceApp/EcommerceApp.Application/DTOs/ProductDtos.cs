namespace EcommerceApp.Application.DTOs;

public record CreateProductDto(string Name, string Description, decimal Price, int Stock);
public record UpdateProductDto(string Name, string Description, decimal Price);
public record ProductResponseDto(Guid Id, string Name, string Description, decimal Price, int Stock, bool IsActive);