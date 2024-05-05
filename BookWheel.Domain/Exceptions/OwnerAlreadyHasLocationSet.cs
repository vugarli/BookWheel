namespace BookWheel.Domain.Exceptions;

public class OwnerAlreadyHasLocationSet : DomainException
{
    public OwnerAlreadyHasLocationSet()
    :base("Owner has already a location set!")
    {
        
    }
}