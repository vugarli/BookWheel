using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Domain.Entities;

namespace BookWheel.Domain.AggregateRoots
{
    public enum ApplicationUserType
    {
        Owner,
        Customer
    }

    public abstract class ApplicationUserRoot : BaseEntity
    {
        protected ApplicationUserRoot
            (
            string name,
            string surname,
            string email,
            ApplicationUserType userType
            )
        {
            Name = name;
            Surname = surname;
            Email = email;
            UserType = userType;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public ApplicationUserType UserType { get; private set; }
    }

    public class CustomerUserRoot : ApplicationUserRoot
    {
        public CustomerUserRoot
            (
            string name,
            string surname,
            string email
            ) : base( name, surname, email, ApplicationUserType.Customer )
        {
        }


        public ICollection<Reservation> Reservations { get; set; }

        public void Reserve(Guid scheduleId)
        {
            if (Reservations.Any(r => r.Status == ReservationStatus.Pending))
                throw new Exception("User already has pending reservation!");

            var newReservation = new Reservation(Id, scheduleId);
            Reservations.Add(newReservation);
        }


    }

    public class OwnerUserRoot : ApplicationUserRoot
    {

        public OwnerUserRoot
            (
            string name,
            string surname,
            string email
            ) : base(name, surname, email, ApplicationUserType.Owner)
        {
        }

        public ICollection<Schedule> Schedules { get; set; }
        public Location Location { get; set; }

        public void AddSchedule(DateTime dateTime)
        {

            if (Schedules
                .Any(s => s.ScheduleDate == dateTime || dateTime < DateTime.UtcNow))
                throw new Exception("Invalid date");

            var schedule = new Schedule(Id,dateTime);

            Schedules.Add(schedule);
        }



    }



}
