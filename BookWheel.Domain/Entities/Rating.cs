using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Entities
{
    public class Rating : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public string? Comment { get; set; }
        public int StarCount { get; set; }
    }
}
