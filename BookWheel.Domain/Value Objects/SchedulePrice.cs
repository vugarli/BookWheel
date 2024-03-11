using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Value_Objects
{
    public record SchedulePrice
    {
        public decimal Amount { get; set; }
    }
}
