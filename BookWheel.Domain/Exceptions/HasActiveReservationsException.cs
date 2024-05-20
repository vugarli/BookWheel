using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class HasActiveReservationsException
        : DomainConflictException
    {
        public HasActiveReservationsException()
            : base("Location has active reservations conflicting with the request.")
        {
            
        }
    }
}
