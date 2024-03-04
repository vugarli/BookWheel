using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Entities
{
    public enum ApplicationUserType
    { 
        Owner,
        Customer
    }

    public class ApplicationUser : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public ApplicationUserType UserType { get; set; }
    }
}
