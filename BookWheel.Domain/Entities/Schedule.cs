using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Domain.AggregateRoots;

namespace BookWheel.Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public Schedule(Guid ownerId, DateTime scheduleDate)
        {
            OwnerId = ownerId;
            ScheduleDate = scheduleDate;
        }

        public Guid OwnerId { get; set; }
        public OwnerUserRoot Owner { get; set; }

        public DateTime ScheduleDate { get; set; }

        public bool IsReserved { get; set; } = false;



        public byte[] Version { get; set; } // optimistic lock

        public void Reserve()
        {
            IsReserved = true;
        }
    }
}
