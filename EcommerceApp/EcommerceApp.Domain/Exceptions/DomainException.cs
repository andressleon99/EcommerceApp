namespace EcommerceApp.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entity, Guid id)
        : base($"{entity} con Id '{id}' no encontrado.") { }
}