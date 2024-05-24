using BookWheel.Domain.Value_Objects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Events
{
    public record ReservationCancelledCustomerEvent : BaseDomainEvent
    {
        public ReservationCancelledCustomerEvent(Guid customerId, Guid ownerId, Guid reservationId, string locationName, TimeRange reservationTimeRange)
        {
            CustomerId = customerId;
            OwnerId = ownerId;
            ReservationId = reservationId;
            LocationName = locationName;
            ReservationTimeRange = reservationTimeRange;
        }

        public Guid CustomerId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ReservationId { get; set; }
        public string LocationName { get; set; }
        public TimeRange ReservationTimeRange { get; set; }
    }
}
