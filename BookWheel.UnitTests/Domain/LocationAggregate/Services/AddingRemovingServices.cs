using BookWheel.Domain.Exceptions;
using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;
using BookWheel.UnitTests.Builders;

namespace BookWheel.UnitTests.Domain.LocationAggregate.Services;

public class AddingRemovingServices
{
    
    [Fact]
    public void ThrowsExceptionWhenAddingDuplicateService()
    {
        var locationId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var service = ServiceProvider.GetService(serviceId,locationId,34,34m);

        var location = new LocationBuilder(Guid.NewGuid())
            .WithId(Guid.NewGuid())
            .WithServices(new List<Service> { service })
            .Build();

        void Action() => location.AddService(service);

        Assert.Throws<DuplicateServiceException>(Action);
    }

    [Fact]
    public void ThrowsExceptionWhenRemovingUsedService()
    {
        var locationId = Guid.NewGuid();
        var service = ServiceProvider.GetService(Guid.NewGuid(),locationId,30,30m);

        var location = new LocationBuilder(Guid.NewGuid())
            .WithId(locationId)
            .WithServices(new List<Service> { service })
            .Build();

        var userId = Guid.NewGuid();

        var services = new List<Service>() { service };



        location.AddReservation(userId,services,LocationContext.GetValidReservationDate(location));

        void Action() => location.RemoveService(service.Id);
        Assert.Throws<ServiceAssociatedWithReservationException>(Action);
    }
    
    
}