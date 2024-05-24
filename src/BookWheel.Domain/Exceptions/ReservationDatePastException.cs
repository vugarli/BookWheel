using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class ReservationDatePastException : DomainConflictException
    {
        public ReservationDatePastException()
            : base("Reservation date is past.")
        {
            
        }
    }
}
