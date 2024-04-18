using BookWheel.Domain.Exceptions;
using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;
using BookWheel.UnitTests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Domain.LocationAggregate.ReservationTests
{
    public class Reservation_Create
    {

        public Location Location { get; init; }

        public TimeOnlyRange WorkingHours = new TimeOnlyRange(TimeOnly.FromDateTime(DateTime.Now), TimeOnly.FromDateTime(DateTime.Now.AddHours(4)));

        public Reservation_Create()
        {
            Location = new Location("Location", Guid.NewGuid(), 34.56, 23.34, 2, WorkingHours);
        }

        [Fact]
        public void ThrowsExceptionOutOfBusinessHours()
        {
            var reservationInterval = new TimeRange(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(5));

            void Action() => Location.AddReservation(Guid.NewGuid(), reservationInterval);

            Assert.Throws<ReservationOutOfBusinessHoursException>(Action);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ThrowsExceptionIfReservationOverlaps_WithBoxCounts(int boxCount)
        {
            var location = new Location("LocationA",Guid.NewGuid(),34.34,34.23,boxCount,new TimeOnlyRange("01:00","23:59"));

            TimeRange[] reservationRanges = ReservationTimeRangeProvider.GetOverlappingReservationTimeRangesForBoxCount(boxCount);
            
            void Action() 
            {
                foreach (var reservationRange in reservationRanges)
                { 
                    location.AddReservation(Guid.NewGuid(),reservationRange);
                }
            }; 

            Assert.Throws<ReservationOverlapsException>(Action);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DoesNotThrowExceptionIfReservationDoesNotOverlap_WithBoxCounts(int boxCount)
        {
            var location = new Location("LocationA", Guid.NewGuid(), 34.34, 34.23, boxCount, new TimeOnlyRange("01:00", "23:59"));

            TimeRange[] reservationRanges = ReservationTimeRangeProvider.GetNonOverlappingReservationTimeRangesForBoxCount(boxCount);

            void Action()
            {
                foreach (var reservationRange in reservationRanges)
                {
                    location.AddReservation(Guid.NewGuid(), reservationRange);
                }
            };

            var exception = Record.Exception(Action);
            Assert.Null(exception);
        }

    }
}
