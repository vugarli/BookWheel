using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class LocationShouldBeClosedException
        : DomainConflictException
    {
        public LocationShouldBeClosedException()
            :base("To perform this request location needs to be closed first. Consider closing location.") { }
    }
}
