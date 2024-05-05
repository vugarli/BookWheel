using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.LocationServices.Dtos
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int MinuteDuration { get; set; }
    }
}
