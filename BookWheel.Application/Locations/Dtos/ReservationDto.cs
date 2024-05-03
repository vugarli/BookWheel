using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Dtos
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public Guid UserId { get; set; }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }

        public int Status { get; set; }

        public int BoxNumber { get; set; }
        public decimal AmountDue { get; set; }
        public int PaymentStatus { get; set; }

        public DateTime FinishedAt { get; set; }
        public DateTime CancelledAt { get; set; }
    }
}
