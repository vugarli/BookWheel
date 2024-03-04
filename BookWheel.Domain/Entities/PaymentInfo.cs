using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Entities
{
    public enum PaymentStatus
    { 
        Refunded,
        Success
    }


    public class PaymentInfo : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
