using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class ReservationNotFoundException
        : DomainNotFoundException
    {
        public ReservationNotFoundException(Guid reservationId)
            :base($"Reservation({reservationId}) not found.")
        {
            
        }
    }
}
