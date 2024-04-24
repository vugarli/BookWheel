using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;
using BookWheel.UnitTests.Builders;

namespace BookWheel.UnitTests.Domain.LocationAggregate;

public class LocationContext
{

    public Location Location { get; set; }
    public static Guid LocationId
    {
        get => Guid.Parse("22cbb167-4f28-41e9-a927-fa750503cbb3");
    }

    public static List<Service> Services { get; set; } = new();
    
    
    static LocationContext()
    {
        Services.Add(ServiceProvider.GetService("e1c38ced-9248-4180-bace-2b605453f05e",LocationId,5));
        Services.Add(ServiceProvider.GetService("e0a92fed-2fe1-4bb8-aa3f-55939f82c354",LocationId,5));
        Services.Add(ServiceProvider.GetService("c9dc23bf-786e-403a-9773-57a689ad2af8",LocationId,10));
        Services.Add(ServiceProvider.GetService("f0c47b03-15e2-4f95-bfc3-24e885c4dc8b",LocationId,15));
        Services.Add(ServiceProvider.GetService("43d8bb6e-7e30-47e1-aa35-fb2031830fa1",LocationId,15));
        Services.Add(ServiceProvider.GetService("32c2d69f-e18f-46fb-b71f-e04253952a9b",LocationId,20));
        Services.Add(ServiceProvider.GetService("b715b3fe-e113-453a-ade5-12925813761e",LocationId,60));
        Services.Add(ServiceProvider.GetService("4cfe0fa9-2ae1-43b9-9a8e-cd954ad2ef9f",LocationId,120));
    }

   
    
    public IEnumerable<(DateTimeOffset, IEnumerable<Service>)> GetReservationWithDuplicateServices()
        => new (DateTimeOffset, IEnumerable<Service>)[]
        {
            (DateTime.Parse("2023-03-03 10:00"),GetDuplicateServices()),
        };
    
    public IEnumerable<(DateTimeOffset, IEnumerable<Service>)> GetReservationWithNonExistentServices()
        => new (DateTimeOffset, IEnumerable<Service>)[]
        {
            (DateTime.Parse("2023-03-03 10:00"),GetNonExistentService()),
        };

    public IEnumerable<(DateTimeOffset, IEnumerable<Service>)> GetConflictingReservations()
        => new (DateTimeOffset, IEnumerable<Service>)[]
        {
            (DateTime.Parse("2023-03-03 10:00"),Get30MinServices()),
            (DateTime.Parse("2023-03-03 10:20"),Get30MinServices()),
        };
    
    public IEnumerable<(DateTimeOffset, IEnumerable<Service>)> GetNonConflictingReservations()
        => new (DateTimeOffset, IEnumerable<Service>)[]
        {
            (DateTime.Parse("2023-03-03 10:00"),Get30MinServices()),
            (DateTime.Parse("2023-03-03 11:00"),Get30MinServices()),
        };



    public static List<Service> Get30MinServices()
    {
        var services = new List<Service>();
        
        services.Add(Services.Single(s=>s.Id == Guid.Parse("f0c47b03-15e2-4f95-bfc3-24e885c4dc8b")));
        services.Add(Services.Single(s=>s.Id == Guid.Parse("43d8bb6e-7e30-47e1-aa35-fb2031830fa1")));

        return services;
    }

    
    public List<Service> Get90MinServices()
    {
        var services = new List<Service>();
        
        services.Add(Services.Single(s=>s.Id == Guid.Parse("b715b3fe-e113-453a-ade5-12925813761e")));
        services.Add(Services.Single(s=>s.Id == Guid.Parse("f0c47b03-15e2-4f95-bfc3-24e885c4dc8b")));
        services.Add(Services.Single(s=>s.Id == Guid.Parse("43d8bb6e-7e30-47e1-aa35-fb2031830fa1")));

        return services;
    }
    
    public List<Service> GetDuplicateServices()
    {
        var services = new List<Service>();
        
        services.Add(Services.Single(s=>s.Id == Guid.Parse("b715b3fe-e113-453a-ade5-12925813761e")));
        services.Add(Services.Single(s=>s.Id == Guid.Parse("b715b3fe-e113-453a-ade5-12925813761e")));

        return services;
    }

    public List<Service> GetNonExistentService()
    {
        var services = new List<Service>();
        
        services.Add(ServiceProvider.GetService("b719b3fe-e113-453a-ade5-12925813761e",Guid.NewGuid(),4));

        return services;
    }


}