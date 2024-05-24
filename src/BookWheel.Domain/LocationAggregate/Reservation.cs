using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Entities;
using BookWheel.Domain.Value_Objects;

namespace BookWheel.Domain.LocationAggregate
{
    
    public enum ReservationStatus
    {
        Finished,
        CustomerCancelled,
        Pending,
        OwnerCancelled
    }

    public class Reservation : BaseEntity<Guid>
    {
        private Reservation() { }
        public Reservation
            (
            Guid id,
            Guid userId,
            TimeRange reservationTimeInterval,
            Guid locationId,
            int boxNumber,
            List<Service> services
            )
        {
            Id = id;
            UserId = Guard.Against.Default(userId);
            ReservationTimeInterval = Guard.Against.Default(reservationTimeInterval);
            LocationId = Guard.Against.Default(locationId);
            BoxNumber = boxNumber;
            Status = ReservationStatus.Pending;
            Guard.Against.Default(services.Count()); // not empty services
            Services = services;
            PaymentDetails = new PaymentDetails(services.Sum(s=>s.Price));
        }

        public PaymentDetails PaymentDetails { get; set; }

        public List<Service> Services { get; set; } = new();
        public int BoxNumber { get; set; }

        public Guid UserId { get; private set; }

        public TimeRange ReservationTimeInterval { get; private set; }
        public Guid LocationId { get; set; }

        public ReservationStatus Status { get; private set; }

        public DateTime FinishedAt { get; private set; }
        public DateTime CancelledAt { get; private set; }

        public void OwnerCancelReservation()
        {
            // if finished no cancellation should happen
            Status = ReservationStatus.OwnerCancelled;
            CancelledAt = DateTime.UtcNow;
        }

        public bool IsActive()
        {
            return Status == ReservationStatus.Pending;
        }

        public void CustomerCancelReservation()
        {
            Status = ReservationStatus.CustomerCancelled;
            CancelledAt = DateTime.UtcNow;
        }


    }
}
