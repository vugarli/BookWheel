namespace BookWheel.Domain.Exceptions;

public class DuplicateServiceException : Exception
{
    public DuplicateServiceException()
    :base("Duplicate services provided!")
    {
        
    }
}