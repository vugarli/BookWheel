namespace BookWheel.Domain.Exceptions;

public class OwnerAlreadyHasLocationSet : DomainConflictException
{
    public OwnerAlreadyHasLocationSet()
    :base("Owner has already a location set!")
    {
        
    }
}