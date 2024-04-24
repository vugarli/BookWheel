namespace BookWheel.Domain.Exceptions;

public class ServiceDoesNotExistException : Exception
{
    public ServiceDoesNotExistException()
    :base("Service does not exist!")
    {
        
    }
}