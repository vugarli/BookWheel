using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Dtos
{
    public class RatingDto
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }
        public Guid LocationId { get; set; }
        public string Comment { get; set; }
        public int StarCount { get; set; }

    }
}
