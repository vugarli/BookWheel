using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Commands
{
    public class SetLocationCommand 
        : IRequest
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
