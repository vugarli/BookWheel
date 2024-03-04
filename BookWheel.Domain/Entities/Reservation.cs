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

    public class Reservation
    {
        public Guid UserId { get; set; }
        public Guid ScheduleId { get; set; }

        public ReservationStatus Status { get; set; }

        public Guid OwnerRatingId { get; set; }
        public Guid CustomerRatingId { get; set; }

        public DateTime FinishedAt { get; set; }
        public DateTime CancelledAt { get; set; }
    }
}
