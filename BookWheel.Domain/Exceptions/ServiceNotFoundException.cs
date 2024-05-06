namespace BookWheel.Domain.Exceptions;

public class ServiceNotFoundException : DomainNotFoundException
{
    public ServiceNotFoundException(Guid Id)
    :base($"Service({Id}) not found")
    {
        
    }
}