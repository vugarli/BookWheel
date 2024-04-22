﻿using BookWheel.Domain.Entities;
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

        public List<Service> Services { get; set; } = new();
        public List<Reservation> Reservations { get; set; } = new();
        //public List<Schedule> Schedules { get; set; }

        public byte[] Version { get; set; }

        public Location
            (
            Guid id,
            string name,
            Guid ownerId,
            double latCoord,
            double longCoord,
            int boxCount,
            TimeOnlyRange workingTimeRange
            )
        {
            Id = id;
            Name = Guard.Against.NullOrEmpty(name);
            OwnerId = Guard.Against.Default(ownerId);
            // Coordinates = new Point
            //     (
            //     Guard.Against.Default(longCoord),
            //     Guard.Against.Default(latCoord)
            //     );
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            // Create a point at Aurich (lat=53.4837, long=7.5404)
            Coordinates = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(7.5404, 53.4837));
            
            BoxCount = boxCount;
            WorkingTimeRange = workingTimeRange;
        }

        public void AddReservation(
            Guid userId,
            List<Service> services,
            DateTimeOffset startDate
        )
        {
            Guard.Against.DuplicateService(services);
            Guard.Against.ServiceDoesNotExist(this,services);
            
            var durationInMinutes = services.Sum(s=>s.MinuteDuration);
            var reservationTimeInterval = new TimeRange(startDate,TimeSpan.FromMinutes(durationInMinutes));
            
            Guard.Against.OutOfBusinessHours(reservationTimeInterval, this);
            
            var overlappingReservations = GetOverlappingReservations(reservationTimeInterval);

            if (overlappingReservations.Count() != 0)
            {
                if (overlappingReservations.Select(r => r.BoxNumber).Distinct().Count() == BoxCount)
                    throw new ReservationOverlapsException();

                var newBoxNumber = overlappingReservations
                    .Select(r => r.BoxNumber)
                    .Count() + 1;

                Reservation newReservation = 
                    new Reservation
                    (
                        Guid.NewGuid(),
                        userId,
                        reservationTimeInterval,
                        Id,
                        newBoxNumber,
                        services.ToList()
                    );

                Reservations.Add(newReservation);
            }
            else
            {
                Reservation newReservation = new Reservation(Guid.NewGuid(),userId, reservationTimeInterval, Id, 1,services.ToList());
                Reservations.Add(newReservation);
            }

            // event add
        }

        public void CancelReservationOwner(Guid reservationId)
        {
            Guard.Against.Default(reservationId);
            
            var reservation = Reservations.SingleOrDefault(r=>r.Id == reservationId);

            if (reservation is not null)
            {
                reservation.OwnerCancelReservation();
                // event
            }
        }
        
        public void CancelReservationCustomer(Guid reservationId)
        {
            Guard.Against.Default(reservationId);

            var reservation = Reservations.SingleOrDefault(r=>r.Id == reservationId);

            if (reservation is not null)
            {
                reservation.CustomerCancelReservation();
                // event
            }

        }
        
        public bool DoesOverlapsReservation(Reservation reservation)
        {
            return Reservations.Any(r => r.BoxNumber == reservation.BoxNumber && r.ReservationTimeInterval.DoesOverlap(reservation.ReservationTimeInterval));
        }

        public List<Reservation> GetOverlappingReservations(TimeRange reservationInterval)
        { 
            return Reservations
                .Where(r=>r.IsActive())
                .Where(r => r.ReservationTimeInterval.DoesOverlap(reservationInterval))
                .ToList();
        }

    }
}
