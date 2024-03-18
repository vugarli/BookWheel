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
        public string Lat { get; set; }
        public string Long { get; set; }
        public string OwnerName { get; set; }
    }
}
