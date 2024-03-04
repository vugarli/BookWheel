using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Reservations
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ReservationStatus { get; set; }
        public int MyProperty { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public DateTime CancelledAt { get; set; }
        public decimal AmountPaid { get; set; }
        public string LocationName { get; set; }
        public string OwnerName { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
