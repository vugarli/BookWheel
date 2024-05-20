using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public class BoxCountIsInUseException
        : DomainConflictException
    {
        public BoxCountIsInUseException()
            :base("Box count is in use. Can not process the request.")
        {
            
        }
    }
}
