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
    public class CreatingReservations : IClassFixture<LocationContext>
    {
        public LocationContext LocationContext { get; set; }

        public CreatingReservations(LocationContext locationContext)
        {
            LocationContext = locationContext;
        }

        [Fact]
        public void ThrowsExceptionOutOfBusinessHours()
        {
            var locationBuilder = new LocationBuilder(Guid.NewGuid());
            var location = locationBuilder
                .WithWorkingTimeRange(new TimeOnlyRange("09:00", "18:00"))
                .WithServices(LocationContext.Get30MinServices())
                .Build();

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
            var locationBuilder = new LocationBuilder(Guid.NewGuid());
            
            var location = locationBuilder
                .WithBoxCount(boxCount)
                .WithWorkingTimeRange(new TimeOnlyRange("01:00", "23:59"))
                .WithServices(LocationContext.Get30MinServices())
                .Build();
            
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
            var builder = new LocationBuilder(Guid.NewGuid());

            var location = builder
                .WithBoxCount(boxCount)
                .WithWorkingTimeRange(new TimeOnlyRange("01:00","23:59"))
                .WithServices(LocationContext.Get30MinServices())
                .Build();
            
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
            var builder = new LocationBuilder(Guid.NewGuid());
            var location = builder.WithWorkingTimeRange(new TimeOnlyRange("01:00", "23:59")).Build();
            
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
            var builder = new LocationBuilder(Guid.NewGuid());
            var location = builder.WithWorkingTimeRange(new TimeOnlyRange("01:00","23:59")).Build();
            
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
