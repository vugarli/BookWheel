namespace BookWheel.Domain.Exceptions;

public class DuplicateServiceException : DomainException
{
    public DuplicateServiceException()
    :base("Duplicate services provided!")
    {
        
    }
}