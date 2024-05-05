using Ardalis.GuardClauses;
using BookWheel.Domain.Entities;

namespace BookWheel.Domain.LocationAggregate;

public class Service : BaseEntity<Guid>
{
    public Guid LocationId { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int MinuteDuration { get; private set; }

    private Service(){}
    
    public Service
        (
            Guid id,
            string name,
            string description,
            decimal price,
            int minuteDuration,
            Guid locationId
            )
    {
        Guard.Against.NullOrEmpty(id);
        Guard.Against.NullOrEmpty(name);
        Guard.Against.NullOrEmpty(description);
        Guard.Against.Negative(price);
        Guard.Against.Negative(minuteDuration);
        Guard.Against.Default(locationId);
        
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        MinuteDuration = minuteDuration;
        LocationId = locationId;
    }
    
    
    
}