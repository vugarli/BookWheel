using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using BookWheel.Domain.LocationAggregate;

namespace BookWheel.UnitTests.Builders;

public static class ServiceProvider
{

    public static Service GetService(Guid id,Guid locationId, int duration, decimal price = 0.0m)
        => new Service(
            id,
            $"Test Service{Guid.NewGuid().ToString().Substring(0, 4)}",
            "Description",
            price,
            duration,
            locationId
        );
    
    public static Service GetService(string id,Guid locationId, int duration, decimal price = 0.0m)
        => GetService(Guid.Parse(id),locationId, duration, price);
    

}