using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Entities
{
    // value object
    public class Location : BaseEntity
    {
        public Guid OwnerId { get; set; }
        public OwnerUser Owner { get; set; }

        public Point Coordinates { get; set; }
        public string Name { get; set; }
    }
}
