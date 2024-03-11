using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Entities;
using BookWheel.Domain.Value_Objects;

namespace BookWheel.Domain.LocationAggregate
{
    public class Schedule : BaseEntity<Guid>
    {
        private Schedule() {}

        public Schedule(Guid locationId, TimeRange scheduleDate,SchedulePrice schedulePrice)
        {
            LocationId = Guard.Against.Default(locationId);
            ScheduleTimeRange = scheduleDate;
            SchedulePrice = schedulePrice;
        }

        public Guid LocationId { get; set; }

        public TimeRange ScheduleTimeRange { get; set; }

        public bool IsReserved { get; set; } = false;

        public byte[] Version { get; set; } // optimistic lock
        public SchedulePrice SchedulePrice { get; private set; }

        public void Reserve()
        {
            IsReserved = true;
        }
    }
}
