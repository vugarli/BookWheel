using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class LocationIsClosedException
        : DomainConflictException
    {
        public LocationIsClosedException() :base("Location is not closed. Check back later."){ }
    }
}
