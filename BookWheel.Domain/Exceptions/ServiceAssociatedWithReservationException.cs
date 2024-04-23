namespace BookWheel.Domain.Exceptions;

public class ServiceAssociatedWithReservationException : Exception
{
    public ServiceAssociatedWithReservationException()
        :base("Cannot delete service. Service is associated with a reservation.") 
    {
        
    }
    
}