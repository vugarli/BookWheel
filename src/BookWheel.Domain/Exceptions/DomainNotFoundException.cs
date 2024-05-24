using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Exceptions
{
    public abstract class DomainNotFoundException : Exception
    {
        protected DomainNotFoundException(string message):base(message)
        {
            
        }
    }
}
