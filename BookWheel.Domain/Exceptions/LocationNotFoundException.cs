using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class LocationNotFoundException
        : DomainNotFoundException
    {
        public LocationNotFoundException(Guid Id)
            : base($"Location({Id}) not found.")
        {
            
        }

    }
}
