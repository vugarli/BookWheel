using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Domain.AggregateRoots;

namespace BookWheel.Domain.Entities
{
    public enum ReservationStatus
    { 
        Finished,
        CustomerCancelled,
        Pending,
        OwnerCancelled
    }

    public class Reservation : BaseEntity
    {
        public Reservation(Guid userId,Guid scheduleId)
        {
            UserId = userId;
            ScheduleId = scheduleId;
            Status = ReservationStatus.Pending;
        }

        public PaymentDetails PaymentDetails { get; set; }

        public Guid UserId { get; private set; }
        public CustomerUserRoot User { get; private set; }
        
        public Guid ScheduleId { get; private set; }
        public Schedule Schedule { get; private set; }

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
