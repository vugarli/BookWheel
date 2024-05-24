using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        public string LocationName { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int BoxCount { get; set; }
        public int Rating { get; set; }
        public bool IsClosed { get; set; }
    }
}
