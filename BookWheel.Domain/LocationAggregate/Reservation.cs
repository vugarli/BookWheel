using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Entities;

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
        private Reservation()
        {
            
        }
        public Reservation
            (
            Guid userId,
            Guid scheduleId,
            Guid locationId
            )
        {
            UserId = Guard.Against.Default(userId);
            ScheduleId = Guard.Against.Default(scheduleId);
            LocationId = Guard.Against.Default(locationId);
            Status = ReservationStatus.Pending;
        }

        public PaymentDetails PaymentDetails { get; set; }

        public Guid UserId { get; private set; }

        public Guid ScheduleId { get; private set; }
        public Guid LocationId { get; set; }

        public ReservationStatus Status { get; private set; }

        public DateTime FinishedAt { get; private set; }
        public DateTime CancelledAt { get; private set; }

        public void OwnerCancelReservation()
        {
            Status = ReservationStatus.OwnerCancelled;
            CancelledAt = DateTime.UtcNow;
        }

        public void CustomerCancelReservation()
        {
            Status = ReservationStatus.CustomerCancelled;
            CancelledAt = DateTime.UtcNow;
        }


    }
}
