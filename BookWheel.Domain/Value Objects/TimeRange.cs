using Ardalis.GuardClauses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Value_Objects
{
    public record TimeRange
    {
        public DateTimeOffset Start { get; private set; }
        public DateTimeOffset End { get; private set; }


        public TimeRange(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            Guard.Against.EqualDateParts(startDate, endDate);
            Guard.Against.OutOfRange(startDate, "start", startDate, endDate);

            Start = startDate;
            End = endDate;
        }


        public TimeRange(DateTimeOffset start, TimeSpan duration)
        : this(start, start.Add(duration))
        {
        }

        public bool DoesOverlap(TimeRange timeRange)
        {
            if (Start < timeRange.End)
            { 
                return End > timeRange.Start;
            }
            return false;
        }


    }


    public static class TimeRangeGuardExtensions
    {
        public static void EqualDateParts
            (
            this IGuardClause guardClause,
            DateTimeOffset start,
            DateTimeOffset end
            )
        {
            if (start.Date != end.Date)
                throw new ArgumentException("Timerange dates should be same!"); 
        }

        
    
    }
}
