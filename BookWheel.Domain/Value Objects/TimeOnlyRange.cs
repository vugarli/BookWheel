using Ardalis.GuardClauses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Value_Objects
{
    public class TimeOnlyRange
    {

        public TimeOnly Start{ get; set; }
        public TimeOnly End{ get; set; }


        public TimeOnlyRange(TimeOnly startTime, TimeOnly endTime)
        {
            Guard.Against.OutOfRange(startTime, "start", startTime, endTime);

            Start = startTime;
            End = endTime;
        }


        public TimeOnlyRange(TimeOnly start, TimeSpan duration)
        : this(start, start.Add(duration))
        {
        }

        public bool DoesOverlap(TimeOnlyRange timeRange)
        {
            if (Start < timeRange.End)
            {
                return End > timeRange.Start;
            }
            return false;
        }

        public bool DoesOverlap(TimeRange timeRange)
        {
            if (Start < TimeOnly.FromDateTime(timeRange.End.DateTime))
            {
                return End > TimeOnly.FromDateTime(timeRange.Start.DateTime);
            }
            return false;
        }


    }
}
