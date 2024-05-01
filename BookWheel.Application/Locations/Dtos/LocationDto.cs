using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Dtos
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int BoxCount { get; set; }
        public int Rating { get; set; }
    }
}
