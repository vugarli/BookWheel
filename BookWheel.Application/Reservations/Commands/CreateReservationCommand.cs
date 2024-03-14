using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Reservations.Commands
{
    public class CreateReservationCommand : IRequest
    {
        public Guid LocationId { get; set; }
        public Guid ScheduleId { get; set; }
    }
}
