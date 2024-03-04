using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public Guid OwnerId { get; set; }
        public DateTime ScheduleDate { get; set; }

        public int Version { get; set; } // optimistic lock
    }
}
