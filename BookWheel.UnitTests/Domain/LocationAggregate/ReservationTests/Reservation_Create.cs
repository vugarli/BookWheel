using BookWheel.Domain.Exceptions;
using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;
using BookWheel.UnitTests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Domain.LocationAggregate.ReservationTests
{
    public class Reservation_Create : IClassFixture<LocationContext>
    {
        public LocationContext LocationContext { get; set; }

        public Reservation_Create(LocationContext locationContext)
        {
            LocationContext = locationContext;
        }

        [Fact]
        public void ThrowsExceptionOutOfBusinessHours()
        {
            var location = LocationContext.GetLocation(1, "09:00", "18:00");
            void Action() => location
                .AddReservation(
                    Guid.NewGuid(),
                    LocationContext.Get30MinServices(),
                    DateTimeOffset.Parse("2023-03-03 19:00"));

            Assert.Throws<ReservationOutOfBusinessHoursException>(Action);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ThrowsExceptionIfReservationOverlaps_WithBoxCounts(int boxCount)
        {
            var location = LocationContext.GetLocation(boxCount,"01:00","23:59"); 
            
            void Action() 
            {
                foreach (var _ in Enumerable.Range(1,boxCount))
                foreach ((DateTimeOffset startDate,IEnumerable<Service> services) in LocationContext.GetConflictingReservations())
                { 
                    location.AddReservation(Guid.NewGuid(),services.ToList(),startDate);
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
            var location = LocationContext.GetLocation(boxCount,"01:00","23:59");

            void Action() 
            {
                foreach (var _ in Enumerable.Range(1,boxCount))
                foreach ((DateTimeOffset startDate,IEnumerable<Service> services) in LocationContext.GetNonConflictingReservations())
                { 
                    location.AddReservation(Guid.NewGuid(),services.ToList(),startDate);
                }
            }; 

            var exception = Record.Exception(Action);
            Assert.Null(exception);
        }

        [Fact]
        public void ThrowsExceptionWhenProvidedDuplicateServices()
        {
            var location = LocationContext.GetLocation(1,"01:00","23:59"); 
            
            void Action() 
            {
                foreach ((DateTimeOffset startDate,IEnumerable<Service> services) in LocationContext.GetReservationWithDuplicateServices())
                { 
                    location.AddReservation(Guid.NewGuid(),services.ToList(),startDate);
                }
            }; 

            Assert.Throws<DuplicateServiceException>(Action);
        }
        
        [Fact]
        public void ThrowsExceptionWhenProvidedNonExistentServices()
        {
            var location = LocationContext.GetLocation(1,"01:00","23:59"); 
            
            void Action() 
            {
                foreach ((DateTimeOffset startDate,IEnumerable<Service> services) in LocationContext.GetReservationWithNonExistentServices())
                { 
                    location.AddReservation(Guid.NewGuid(),services.ToList(),startDate);
                }
            }; 

            Assert.Throws<ServiceDoesNotExistException>(Action);
        }


    }
}
