using Ardalis.GuardClauses;
using BookWheel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.LocationAggregate.Extensions
{
    public static class LocationGuardExtensions
    {
        public static void OverlappingScheduleDates
            (
            this IGuardClause guardClause,
            Schedule schedule,
            List<Schedule> schedulesList
            )
        {
            if (schedulesList.Any(s => s.ScheduleTimeRange.DoesOverlap(schedule.ScheduleTimeRange)))
                throw new OverlappingScheduleException();
        }

        public static void DuplicateSchedules
            (
            this IGuardClause guardClause,
            Schedule schedule,
            List<Schedule> schedulesList
            )
        {
            if (schedulesList.Any(s => s.Id == schedule.Id))
                throw new DuplicateScheduleException();
        }


    }
}
