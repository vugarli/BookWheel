using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Services;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Domain.Value_Objects;
using BookWheel.Infrastructure;
using BookWheel.Infrastructure.Repositories;
using BookWheel.UnitTests.Builders;
using Xunit;

namespace BookWheel.IntegrationTests.OwnerTests;

public class SetLocationAndConfirm : IClassFixture<SharedDatabaseFixture>
{
    private readonly SharedDatabaseFixture _sharedDatabaseFixture;

    public SetLocationAndConfirm(SharedDatabaseFixture sharedDatabaseFixture)
    {
        _sharedDatabaseFixture = sharedDatabaseFixture;
    }

    [Fact]
    public async Task SuccessfullySetsLocationToOwner()
    {
        using (var transaction = await _sharedDatabaseFixture.DbConnection.BeginTransactionAsync())
        {
            var wContext = _sharedDatabaseFixture.CreateContext(transaction);
            var rContext = _sharedDatabaseFixture.CreateContext(transaction);
            var unitOfWork = new UnitOfWork(wContext);
            var userRepo = new UserRepository(wContext);
            var wLocationRepo = new LocationRepository(wContext);
            var rLocationRepo = new LocationRepository(rContext);
            
            var service = new OwnerLocationSetter(userRepo,wLocationRepo,unitOfWork);
            
            var ownerId = Guid.NewGuid();
            var locationId = Guid.NewGuid();
            var owner = OwnerProvider.GetOwner(ownerId,"a","a");
            
            var location = LocationProvider.GetDefaultLocation(locationId,ownerId);
            
            await userRepo.CreateOwnerAsync(owner);
            await wContext.SaveChangesAsync();

            await service.SetLocationToOwnerAsync(ownerId,location,CancellationToken.None);

            var spec = new GetLocationByOwnerSpecification(ownerId);
            var locationExists = await rLocationRepo.CheckLocationBySpecificationAsync(spec);
            
            Assert.True(locationExists);
        }
    }
    
    [Fact]
    public async Task ThrowsExceptionWhenSettingLocationTwiceToOwner()
    {
        using (var transaction = await _sharedDatabaseFixture.DbConnection.BeginTransactionAsync())
        {
            var wContext = _sharedDatabaseFixture.CreateContext(transaction);
            var unitOfWork = new UnitOfWork(wContext);
            var userRepo = new UserRepository(wContext);
            var wLocationRepo = new LocationRepository(wContext);
            
            var service = new OwnerLocationSetter(userRepo,wLocationRepo,unitOfWork);
            
            var ownerId = Guid.NewGuid();
            var locationId = Guid.NewGuid();
            var locationId2 = Guid.NewGuid();
            var owner = OwnerProvider.GetOwner(ownerId,"a","a");
            
            var location1 = LocationProvider.GetDefaultLocation(locationId,ownerId);
            var location2 = LocationProvider.GetDefaultLocation(locationId2,ownerId);
            
            await userRepo.CreateOwnerAsync(owner);
            await wContext.SaveChangesAsync();

            async Task Action()
            {
                await service.SetLocationToOwnerAsync(ownerId,location1,CancellationToken.None);
                await service.SetLocationToOwnerAsync(ownerId,location2,CancellationToken.None);
            }
            
            await Assert.ThrowsAsync<OwnerAlreadyHasLocationSet>(Action);
        }
    }
    


}