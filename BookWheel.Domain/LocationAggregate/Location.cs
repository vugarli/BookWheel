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

using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Value_Objects;

namespace BookWheel.Domain.LocationAggregate
{
    public class Location : BaseEntity<Guid>, IAggregateRoot
    {
        private Location() {}
        public string Name { get; private set; }
        public Guid OwnerId { get; private set; }
        public Point Coordinates { get; private set; }
        public int BoxCount { get; set; } = 1;
        public TimeOnlyRange WorkingTimeRange { get; set; }
        public List<Reservation> Reservations { get; set; }
        //public List<Schedule> Schedules { get; set; }

        public byte[] Version { get; set; }

        public Location
            (
            string name,
            Guid ownerId,
            double latCoord,
            double longCoord,
            int boxCount,
            TimeOnlyRange workingTimeRange
            )
        {
            Name = Guard.Against.NullOrEmpty(name);
            OwnerId = Guard.Against.Default(ownerId);
            Coordinates = new Point
                (
                Guard.Against.Default(longCoord),
                Guard.Against.Default(latCoord)
                );
            BoxCount = boxCount;
            WorkingTimeRange = workingTimeRange;
        }


        public void AddReservation(
            Guid userId,
            TimeRange reservationTimeInterval
            )
        {
            Guard.Against.OutOfBusinessHours(reservationTimeInterval, this);
            
            var overlappingReservations = GetOverlappingReservations(reservationTimeInterval);

            if (overlappingReservations.Count() != 0)
            {
                if (overlappingReservations.Select(r => r.BoxNumber).Distinct().Count() == BoxCount)
                    throw new ReservationOverlapsException();

                var newBoxNumber = overlappingReservations.Select(r => r.BoxNumber).Max() + 1;

                Reservation newReservation = 
                    new Reservation
                    (
                        userId,
                        reservationTimeInterval,
                        Id,
                        newBoxNumber
                    );

                Reservations.Add(newReservation);
            }
            else
            {
                Reservation newReservation = new Reservation(userId, reservationTimeInterval, Id, 1);
                Reservations.Add(newReservation);
            }

            // event add
        }



        public bool DoesOverlapsReservation(Reservation reservation)
        {
            return Reservations.Any(r => r.BoxNumber == reservation.BoxNumber && r.ReservationTimeInterval.DoesOverlap(reservation.ReservationTimeInterval));
        }

        public List<Reservation> GetOverlappingReservations(TimeRange reservationInterval)
        { 
            return Reservations.Where(r => r.ReservationTimeInterval.DoesOverlap(reservationInterval)).ToList();
        }

    }
}
