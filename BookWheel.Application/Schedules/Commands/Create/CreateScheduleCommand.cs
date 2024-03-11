using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Schedules.Commands.Create
{
    public record CreateScheduleCommand : IRequest
    {
        public Guid LocationId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public decimal Amount { get; set; }
    }
}
