using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xeptions;

namespace BookWheel.Application.Exceptions
{
    public class ModelValidationException 
        : Xeption
    {
        public ModelValidationException(string s = "Validation errors occured. Please check errors for details.")
            : base(s)
        {
            
        }
    }
}
