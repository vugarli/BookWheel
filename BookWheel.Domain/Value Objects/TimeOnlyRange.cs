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
        public TimeOnly Start{ get; private set; }
        public TimeOnly End{ get; private set; }

private TimeOnlyRange()
{
    
}
        public TimeOnlyRange(TimeOnly startTime, TimeOnly endTime)
        {
            Guard.Against.OutOfRange(startTime, "start", startTime, endTime);

            Start = startTime;
            End = endTime;
        }

        public TimeOnlyRange(string start,string end)
            :this(TimeOnly.Parse(start),TimeOnly.Parse(end))
        {
        }

        public TimeOnlyRange(TimeOnly start, TimeSpan duration)
        : this(start, start.Add(duration))
        {
        }

        public TimeOnlyRange WithNewEnd(TimeOnly endTime)
        {
            Guard.Against.OutOfRange(Start, "start", Start, endTime);
            End = endTime;
            return this;
        }

        public int DurationInHours()
        {
            return (End.Hour - Start.Hour);
        }

        public List<TimeOnly> GetHours()
        {
            var timeHours = new List<TimeOnly>();

            for(int i=0;i< DurationInHours() + 1; i++)
            {
                timeHours.Add(new TimeOnly(Start.Hour+i,0));
            }
            return timeHours;
        }


        public bool DoesOverlap(TimeOnlyRange timeRange)
        {
            if (Start < timeRange.End)
            {
                return End > timeRange.Start;
            }
            return false;
        }
        

        public bool DoesContain(TimeOnly timeOnly)
        {
            if (Start <= timeOnly)
            {
                return End >= timeOnly;
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
