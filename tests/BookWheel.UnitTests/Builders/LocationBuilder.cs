using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;

namespace BookWheel.UnitTests.Builders;

public class LocationBuilder
{
    private Guid Id { get; set; } = Guid.NewGuid();
    private string Name { get; set; } = Guid.NewGuid().ToString().Substring(0,20);
    private Guid OwnerId { get; set; }
    private int BoxCount { get; set; } = 1;
    private double Latitude { get; set; } = 21.21;
    private double Longitude { get; set; } = 21.21;
    private List<Service> Services { get; set; } = new();
    private TimeOnlyRange WorkingTimeRange { get; set; } = new TimeOnlyRange("09:00","18:00");

    public LocationBuilder WithLatLong
        (
            double latitude,
            double longitude
        )
    {
        Latitude = latitude;
        Longitude = longitude;
        return this;
    }
    
    public LocationBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public LocationBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public LocationBuilder WithWorkingTimeRange
        (
            TimeOnlyRange workingHours
        )
    {
        WorkingTimeRange = workingHours;
        return this;
    }

    public LocationBuilder WithServices(List<Service> services)
    {
        Services.AddRange(services);
        return this;
    }
    
    public LocationBuilder WithBoxCount(int boxCount)
    {
        BoxCount = boxCount;
        return this;
    }
    
    public LocationBuilder(Guid ownerId)
    {
        OwnerId = ownerId;
    }

    public Location Build()
    {
        var location = new Location(Id,Name,OwnerId,Latitude,Longitude,BoxCount,WorkingTimeRange);
        location.Services.AddRange(Services);
        return location;
    }
    
}