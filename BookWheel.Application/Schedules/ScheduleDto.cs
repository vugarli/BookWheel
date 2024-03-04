using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Schedules
{
    public class ScheduleDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public bool Reserved { get; set; }
        public string OwnerName { get; set; }
    }
}
