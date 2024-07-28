using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Services;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Domain.Specifications.Rating;
using BookWheel.Infrastructure;
using BookWheel.Infrastructure.Repositories;
using BookWheel.UnitTests.Builders;
using BookWheel.UnitTests.Domain.LocationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookWheel.IntegrationTests.RatingTests
{
    [Collection("Sequential")]
    public class LocationReservationRateConfirm
        : IClassFixture<SharedDatabaseFixture>
    {
        private SharedDatabaseFixture Fixture { get; }
        public IConfiguration Configuration { get; set; }

        public LocationReservationRateConfirm(SharedDatabaseFixture sharedDatabaseFixture)
        {
            Fixture = sharedDatabaseFixture;
            var mock = new Moq.Mock<IConfiguration>();
            mock.Setup(c=>c.GetSection("ConnectionStrings")["MSSQL"]).Returns(Fixture.CONNECTION_STRING);
            Configuration = mock.Object;
        }
        

        [Fact]
        public async Task SuccessfullRateToReservation()
        {
            using var transaction = Fixture.DbConnection.BeginTransaction();

            var wDbContext =  Fixture.CreateContext(transaction);
            var rDbContext =  Fixture.CreateContext(transaction);
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

            var reservationId = location.AddReservation(customerId, new List<Service>() { service }, LocationContext.GetValidReservationDate(location));

            //

            await wUserRepo.CreateCustomerAsync(customer);
            await wUserRepo.CreateOwnerAsync(owner);
            await wLocationRepo.AddLocationAsync(location);
            await wDbContext.SaveChangesAsync();

            var ratingRepository = new RatingRepository(wDbContext,Configuration);

            var ratingId = Guid.NewGuid();

            var rating = 
                new RatingBuilder(reservationId)
                .WithId(ratingId).Build();
            
            await ratingRepository.AddRatingAsync(rating);

            await wDbContext.SaveChangesAsync();

            var spec = new GetRatingsByLocationId(locationId);

            var ratings = await ratingRepository.GetRatingsBySpecificationAsync(spec);

            //

            Assert.True(ratings.Count() != 0);
            Assert.NotNull(ratings.FirstOrDefault());
            Assert.Equal(ratings[0].ReservationId,reservationId);
        }

    }
}
