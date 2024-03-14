using BookWheel.Domain.Entities;
using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;
using Guard = Ardalis.GuardClauses.Guard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BookWheel.Domain.LocationAggregate.Extensions;
using BookWheel.Domain.Events;

namespace BookWheel.Domain.LocationAggregate
{
    public class Location : BaseEntity<Guid>, IAggregateRoot
    {
        private Location() {}
        public string Name { get; private set; }
        public Guid OwnerId { get; private set; }
        public Point Coordinates { get; private set; }

        public List<Reservation> Reservations { get; set; }
        public List<Schedule> Schedules { get; set; }

        public Location
            (
            string name,
            Guid ownerId,
            double latCoord,
            double longCoord
            )
        {
            Name = Guard.Against.NullOrEmpty(name);
            OwnerId = Guard.Against.Default(ownerId);
            Coordinates = new Point
                (
                Guard.Against.Default(longCoord),
                Guard.Against.Default(latCoord)
                ); ;
        }

        public void AddSchedule(Schedule newSchedule)
        {
            Guard.Against.Null(newSchedule);
            Guard.Against.OverlappingScheduleDates(newSchedule,Schedules);
            Guard.Against.DuplicateSchedules(newSchedule,Schedules);

            Schedules.Add(newSchedule);
            Events.Add(new ScheduleCreatedEvent(newSchedule));
        }

        public void RemoveSchedule(Schedule schedule)
        {
            Guard.Against.Null(schedule);
            //TODO check for posible reservation
            var scheduleDelete = Schedules.FirstOrDefault(s=>s.Id == schedule.Id);

            if (scheduleDelete is not null)
            { 
                Schedules.Remove(scheduleDelete);
                Events.Add(new ScheduleCreatedEvent(scheduleDelete));
            }
        }

        public void AddReservation(Reservation newReservation)
        { 
            
        }




    }
}
