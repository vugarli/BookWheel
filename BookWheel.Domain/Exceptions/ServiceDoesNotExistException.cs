namespace BookWheel.Domain.Exceptions;

public class ServiceDoesNotExistException : DomainNotFoundException
{
    public ServiceDoesNotExistException()
    :base("Service does not exist!")
    {
        
    }
}