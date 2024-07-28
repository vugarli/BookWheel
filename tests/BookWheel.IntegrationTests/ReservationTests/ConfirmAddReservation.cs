using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Infrastructure.Repositories;
using BookWheel.UnitTests.Builders;
using BookWheel.UnitTests.Domain.LocationAggregate;
using Xunit;


namespace BookWheel.IntegrationTests.ReservationTests;
[Collection("Sequential")]
public class ConfirmAddReservation :IClassFixture<SharedDatabaseFixture>
{
    public SharedDatabaseFixture Fixture { get; set; }
    
    public ConfirmAddReservation(SharedDatabaseFixture fixture)
    {
        Fixture = fixture;
    }
    
    [Fact]
    public async Task ReservationAddConfirm()
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
        var location = new LocationBuilder(ownerId).WithId(locationId).WithServices(new List<Service>(){service}).Build();

        var owner = new OwnerBuilder().WithId(ownerId).Build();
        var customer = new CustomerBuilder().WithId(customerId).Build();

        location.AddReservation(customerId,new List<Service>(){service},LocationContext.GetValidReservationDate(location));
        
        //
        
        await wUserRepo.CreateCustomerAsync(customer);
        await wUserRepo.CreateOwnerAsync(owner);
        await wLocationRepo.AddLocationAsync(location);
        await wDbContext.SaveChangesAsync();

        var spec = new GetLocationByIdSpecification(locationId);
        var rLocation = await rLocationRepo.GetLocationBySpecificationAsync(spec);
        
        //
        
        Assert.NotNull(rLocation);
        Assert.NotEmpty(rLocation.ActiveReservations);
        Assert.NotNull(rLocation.ActiveReservations.FirstOrDefault());
        Assert.True(rLocation.ActiveReservations.FirstOrDefault().Status == ReservationStatus.Pending);
    }
    
    
    
}