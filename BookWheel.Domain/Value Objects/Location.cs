using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Entities
{
    public record Location
    {
        public Point Coordinates { get; set; }
        public string Name { get; set; }
    }
}
