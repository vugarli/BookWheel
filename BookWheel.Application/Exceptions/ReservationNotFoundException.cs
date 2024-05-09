using BookWheel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Exceptions
{
    public class ReservationNotFoundException : DomainNotFoundException
    {
        public ReservationNotFoundException()
            : base("Reservation not found")
        {
            
        }

    }
}
