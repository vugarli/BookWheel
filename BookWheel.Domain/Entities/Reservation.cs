using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Entities
{
    public enum ReservationStatus
    { 
        Finished,
        CustomerCancelled,
        Pending,
        OwnerCancelled
    }

    public class Reservation : BaseEntity
    {
        public Guid UserId { get; set; }
        public CustomerUser User { get; set; }
        
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public ReservationStatus Status { get; set; }

        public DateTime FinishedAt { get; set; }
        public DateTime CancelledAt { get; set; }
    }
}
