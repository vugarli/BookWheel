﻿using Ardalis.GuardClauses;
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

        public static void InvalidCoordinates
            (
                this IGuardClause guardClause,
                double latitude,
                double longitude
            )
        {
            // coordinates valid if:
            // -90 <= lat <= 90
            // -180 <= long <= 180
            
            if (latitude is > 90 or < -90)
                throw new ArgumentException($"Latitude({latitude}) is not valid");
            if(longitude is > 180 or < -180)
                throw new ArgumentException($"Longitude({longitude}) is not valid");
        }
        
        
        public static void InUseService
            (this IGuardClause guardClause,
                Location locaiton,
                Guid serviceToDeleteId
                ){
            if (locaiton.GetActiveReservations().SelectMany(r => r.Services).Any(s => s.Id == serviceToDeleteId))
                throw new ServiceAssociatedWithReservationException();
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
                throw new ReservationOverlapsException();
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
        
    }
}
