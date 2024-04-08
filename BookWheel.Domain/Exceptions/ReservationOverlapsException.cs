using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class ReservationOverlapsException : Exception
    {
        public ReservationOverlapsException()
            : base("Reservation overlaps!") 
        {
            
        }
    }
}
