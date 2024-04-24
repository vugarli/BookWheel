using BookWheel.Domain.LocationAggregate;

namespace BookWheel.UnitTests.Builders;

public class ServiceBuilder
{
    private Guid Id { get; set; } = Guid.NewGuid();
    private Guid LocationId { get; init; }
    private string Name { get; set; } = Guid.NewGuid().ToString().Substring(0,10);
    private string Description { get; set; } = Guid.NewGuid().ToString().Substring(0,10);
    private decimal Price { get; set; } = 51m;
    private int MinuteDuration { get; set; } = 5;

    public ServiceBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public ServiceBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public ServiceBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public ServiceBuilder WithPrice(decimal price)
    {
        Price = price;
        return this;
    }

    public ServiceBuilder WithDuration(int minuteDuration)
    {
        MinuteDuration = minuteDuration;
        return this;
    }

    public Service Build()
    {
        return new Service(Id,Name,Description,Price,MinuteDuration,LocationId);
    }
    
    public ServiceBuilder
        (
            Guid locationId
        )
    {
        LocationId = locationId;
    }
    
    

}