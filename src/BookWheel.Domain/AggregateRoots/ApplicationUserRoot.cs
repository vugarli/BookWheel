using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Domain.Entities;
using BookWheel.Domain.LocationAggregate;

namespace BookWheel.Domain.AggregateRoots
{
    public enum ApplicationUserType
    {
        Owner,
        Customer
    }

    public abstract class ApplicationUserRoot : BaseEntity<Guid>
    {
        protected ApplicationUserRoot()
        {
            
        }
        protected ApplicationUserRoot
            (
            string name,
            string surname,
            string email,
            string phoneNumber,
            ApplicationUserType userType
            )
        {
            Name = name;
            Surname = surname;
            Email = email;
            PhoneNumber = phoneNumber;
            UserType = userType;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public ApplicationUserType UserType { get; private set; }
    }

    public class CustomerUserRoot : ApplicationUserRoot
    {
        private CustomerUserRoot()
        {
            
        }
        public CustomerUserRoot
            (Guid id,
            string name,
            string surname,
            string email,
            string phoneNumber
            ) : base( name, surname, email, phoneNumber, ApplicationUserType.Customer )
        {
            Id = id;
        }


        public ICollection<Reservation> Reservations { get; set; }

    }

    public class OwnerUserRoot : ApplicationUserRoot
    {
        private OwnerUserRoot()
        {
            
        }
        public OwnerUserRoot
            (
            Guid id,
            string name,
            string surname,
            string email, 
            string phoneNumber
            ) : base(name, surname, email, phoneNumber, ApplicationUserType.Owner)
        {
            Id = id;
        }
    }
}
