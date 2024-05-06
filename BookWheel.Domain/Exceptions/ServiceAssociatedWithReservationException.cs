namespace BookWheel.Domain.Exceptions;

public class ServiceAssociatedWithReservationException : DomainConflictException
{
    public ServiceAssociatedWithReservationException()
        :base("Cannot delete service. Service is associated with a reservation.") 
    {
        
    }
    
}