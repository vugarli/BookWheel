using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Services;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Domain.Specifications.Rating;
using BookWheel.Infrastructure;
using BookWheel.Infrastructure.Repositories;
using BookWheel.UnitTests.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookWheel.IntegrationTests.RatingTests
{
    public class LocationReservationRateConfirm
        : IClassFixture<SharedDatabaseFixture>
    {
        private SharedDatabaseFixture Fixture { get; }
        public LocationReservationRateConfirm(SharedDatabaseFixture sharedDatabaseFixture)
        {
            Fixture = sharedDatabaseFixture;
        }


        [Fact]
        public async Task SuccessfullRateToReservation()
        {
            using var transaction = Fixture.DbConnection.BeginTransaction();

            var wDbContext = Fixture.CreateContext(transaction);
            var rDbContext = Fixture.CreateContext(transaction);
            var wUserRepo = new UserRepository(wDbContext);
            var wLocationRepo = new LocationRepository(wDbContext);

            var rLocationRepo = new LocationRepository(rDbContext);
            var locationId = Guid.NewGuid();

            var ownerId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var serviceId = Guid.NewGuid();

            var service = new ServiceBuilder(locationId).WithId(serviceId).Build();
            var location = new LocationBuilder(ownerId).WithId(locationId).WithServices(new List<Service>() { service }).Build();

            var owner = new OwnerBuilder().WithId(ownerId).Build();
            var customer = new CustomerBuilder().WithId(customerId).Build();

            var reservationId = location.AddReservation(customerId, new List<Service>() { service }, DateTime.Now);

            //

            await wUserRepo.CreateCustomerAsync(customer);
            await wUserRepo.CreateOwnerAsync(owner);
            await wLocationRepo.AddLocationAsync(location);
            await wDbContext.SaveChangesAsync();

            var ratingRepository = new RatingRepository(wDbContext);

            var ratingId = Guid.NewGuid();

            var rating = 
                new RatingBuilder(locationId,reservationId)
                .WithId(ratingId).Build();
            
            await ratingRepository.AddRatingAsync(rating);

            await wDbContext.SaveChangesAsync();

            var spec = new GetRatingsByLocationId(locationId);

            var ratings = await ratingRepository.GetRatingsBySpecificationAsync(spec);

            //

            Assert.True(ratings.Count() != 0);
            Assert.NotNull(ratings.FirstOrDefault());
            Assert.Equal(ratings[0].LocationId,locationId);
            Assert.Equal(ratings[0].ReservationId,reservationId);
        }

    }
}
