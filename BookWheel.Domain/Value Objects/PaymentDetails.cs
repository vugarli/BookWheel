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
        Success,
        Pending
    }


    public record PaymentDetails
    {
        public decimal AmountDue { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
