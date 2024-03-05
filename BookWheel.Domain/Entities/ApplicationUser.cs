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

    public abstract class ApplicationUser : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public ApplicationUserType UserType { get; set; }
    }

    public class CustomerUser : ApplicationUser
    { 
        public ICollection<Reservation> Reservations { get; set; }
    }

    public class OwnerUser : ApplicationUser 
    {
        public ICollection<Schedule> Schedules { get; set; }
    }



}
