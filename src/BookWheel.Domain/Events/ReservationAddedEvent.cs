using BookWheel.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Events
{
    public record ReservationAddedEvent : BaseDomainEvent
    {
        public ReservationAddedEvent(Guid customerId, Guid ownerId, Guid reservationId, TimeRange reservationTimeRange, string locationName)
        {
            CustomerId = customerId;
            OwnerId = ownerId;
            ReservationId = reservationId;
            ReservationTimeRange = reservationTimeRange;
            LocationName = locationName;
        }

        public Guid CustomerId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ReservationId { get; set; }
        public string LocationName { get; set; }
        public TimeRange ReservationTimeRange { get; set; }
    }
}
