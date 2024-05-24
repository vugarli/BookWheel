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
        public PaymentDetails(decimal amountDue)
        {
            AmountDue = amountDue;
            Status = PaymentStatus.Pending;
        }
        
        public decimal AmountDue { get; private set; }

        public PaymentStatus Status { get; private set; }
    }
}
