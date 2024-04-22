namespace BookWheel.Domain.Exceptions;

public class OwnerAlreadyHasLocationSet : Exception
{
    public OwnerAlreadyHasLocationSet()
    :base("Owner has already a location set!")
    {
        
    }
}