namespace BookWheel.Domain.Exceptions;

public class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(Guid Id)
    :base($"Service({Id}) not found")
    {
        
    }
}