using Ardalis.GuardClauses;
using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Value_Objects;
using NetTopologySuite.LinearReferencing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.LocationAggregate.Extensions
{
    public static class LocationGuardExtensions
    {

        public static void DuplicateService(
            this IGuardClause guardClause,
            IEnumerable<Service> services)
        {
            if (services.Count() != services.Distinct().Count())
            {
                throw new DuplicateServiceException();
            }
        }
        public static void ServiceDoesNotExist(
            this IGuardClause guardClause,
            Location location,
            IEnumerable<Service> services)
        {
            if (!services.All(s => location.Services.Any(ls => ls.Id == s.Id)))
            {
                throw new ServiceDoesNotExistException();
            }
        }

        public static void OverlappingReservations
            (
            this IGuardClause guardClause,
            Reservation reservation,
            Location location
            )
        {

            if (
                location.DoesOverlapsReservation(reservation)
                )
                throw new Exception("Overlapping reservations!");
        }

        public static void OutOfBusinessHours
            (
            this IGuardClause guardClause,
            TimeRange timeRange,
            Location location
            )
        {
            var reservationStart = TimeOnly.FromDateTime(timeRange.Start.DateTime);
            var reservationEnd = TimeOnly.FromDateTime(timeRange.End.DateTime);

            if (reservationStart < location.WorkingTimeRange.Start || reservationEnd > location.WorkingTimeRange.End)
                throw new ReservationOutOfBusinessHoursException();
        }


        //public static void OverlappingScheduleDates
        //    (
        //    this IGuardClause guardClause,
        //    Schedule schedule,
        //    List<Schedule> schedulesList
        //    )
        //{
        //    if (schedulesList.Any(s => s.ScheduleTimeRange.DoesOverlap(schedule.ScheduleTimeRange)))
        //        throw new OverlappingScheduleException();
        //}

        //public static void DuplicateSchedules
        //    (
        //    this IGuardClause guardClause,
        //    Schedule schedule,
        //    List<Schedule> schedulesList
        //    )
        //{
        //    if (schedulesList.Any(s => s.Id == schedule.Id))
        //        throw new DuplicateScheduleException();
        //}


    }
}
