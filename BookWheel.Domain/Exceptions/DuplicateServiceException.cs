namespace BookWheel.Domain.Exceptions;

public class DuplicateServiceException : DomainConflictException
{
    public DuplicateServiceException()
    :base("Duplicate services provided!")
    {
        
    }
}