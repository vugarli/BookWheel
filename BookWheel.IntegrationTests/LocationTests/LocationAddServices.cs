using BookWheel.Domain.Services;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Infrastructure;
using BookWheel.Infrastructure.Repositories;
using BookWheel.UnitTests.Builders;
using Xunit;

namespace BookWheel.IntegrationTests.LocationTests;

public class LocationAddServices : IClassFixture<SharedDatabaseFixture>
{
    public SharedDatabaseFixture Fixture { get; set; }
    
    public LocationAddServices
        (
            SharedDatabaseFixture sharedDatabaseFixture
        )
    {
        Fixture = sharedDatabaseFixture;
    }
    
    [Fact]
    public async Task ConfirmAddServices()
    {
        using (var transaction = Fixture.DbConnection.BeginTransaction())
        {
            var wContext = Fixture.CreateContext(transaction);
            var rContext = Fixture.CreateContext(transaction);
            var wLocationRepo = new LocationRepository(wContext);
            var wOwnerRepo = new UserRepository(wContext);
            var rLocationRepo = new LocationRepository(rContext);
            var userRepo = new UserRepository(wContext);
            var unitOfWork = new UnitOfWork(wContext);
            
            var service = new OwnerLocationSetter(userRepo,wLocationRepo,unitOfWork);
            
            var ownerId = Guid.NewGuid();
            // var owner = OwnerProvider.GetOwner(ownerId,"","");
            var owner = new OwnerBuilder().WithId(ownerId).Build();
            
            await wOwnerRepo.CreateOwnerAsync(owner);
            await wContext.SaveChangesAsync();
            
            var locationId = Guid.NewGuid();
            // var location = LocationProvider.GetDefaultLocation(locationId,ownerId);
            var location = new LocationBuilder(ownerId).WithId(locationId).Build();
            
            var locationServiceId = Guid.NewGuid();
            var locationService = ServiceProvider.GetService(locationServiceId,locationId,30,30.0m);
            
            location.AddService(locationService);
            
            await unitOfWork.SaveChangesAsync();
            
            await service.SetLocationToOwnerAsync(ownerId,location,CancellationToken.None);

            var spec = new GetLocationByIdSpecification(locationId);
            var rLocation = await rLocationRepo.GetLocationBySpecificationAsync(spec);
            transaction.Commit();
            Assert.NotNull(rLocation);
            Assert.NotNull(rLocation.Services);
            Assert.True(rLocation.Services.Count() == 1);
            Assert.NotNull(rLocation.Services.FirstOrDefault());
            Assert.True(rLocation.Services.FirstOrDefault().Id == locationServiceId);
        }
    }

    [Fact]
    public async Task ConfirmDeleteServices()
    {
        using (var transaction = Fixture.DbConnection.BeginTransaction())
        {
            var wContext = Fixture.CreateContext(transaction);
            var rContext = Fixture.CreateContext(transaction);
            var wLocationRepo = new LocationRepository(wContext);
            var wOwnerRepo = new UserRepository(wContext);
            var rLocationRepo = new LocationRepository(rContext);
            var userRepo = new UserRepository(wContext);
            var unitOfWork = new UnitOfWork(wContext);
            
            var service = new OwnerLocationSetter(userRepo,wLocationRepo,unitOfWork);
            
            var ownerId = Guid.NewGuid();
            // var owner = OwnerProvider.GetOwner(ownerId,"","");
            var owner = new OwnerBuilder().WithId(ownerId).Build();
            
            await wOwnerRepo.CreateOwnerAsync(owner);
            await wContext.SaveChangesAsync();
            
            var locationId = Guid.NewGuid();
            //var location = LocationProvider.GetDefaultLocation(locationId,ownerId);
            var location = new LocationBuilder(ownerId).WithId(locationId).Build();
            var locationServiceId = Guid.NewGuid();
            var locationService = ServiceProvider.GetService(locationServiceId,locationId,30,30.0m);
            
            location.AddService(locationService);
            
            await unitOfWork.SaveChangesAsync();
            
            await service.SetLocationToOwnerAsync(ownerId,location,CancellationToken.None);

            var spec = new GetLocationByIdSpecification(locationId);
            
            location = await wLocationRepo.GetLocationBySpecificationAsync(spec);
            
            Assert.NotNull(location);
            Assert.NotNull(location.Services);
            Assert.True(location.Services.Count() != 0);
            
            location.RemoveService(locationServiceId);
            
            await unitOfWork.SaveChangesAsync();
            
            var rLocation = await rLocationRepo.GetLocationBySpecificationAsync(spec);
            
            Assert.NotNull(rLocation);
            Assert.NotNull(rLocation.Services);
            Assert.True(rLocation.Services.Count() == 0);
        }
    }

    
    
    
    
}