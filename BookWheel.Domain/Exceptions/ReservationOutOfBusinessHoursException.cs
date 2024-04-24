using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class ReservationOutOfBusinessHoursException : Exception
    {
        public ReservationOutOfBusinessHoursException()
            :base("Reservation is out of business hours!")
        {
        }
    }
}
