using BookWheel.Domain.LocationAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Events
{
    public record ScheduleCreatedEvent : BaseDomainEvent,INotification
    {
        public ScheduleCreatedEvent(Schedule schedule)
        {
            Schedule = schedule;
        }
        public Schedule Schedule { get; }
    }
}
