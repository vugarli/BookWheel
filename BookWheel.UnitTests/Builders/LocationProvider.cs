using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Builders
{
    public static class LocationProvider
    {
        
        public static Location GetLocation(Guid locationId,int boxCount,TimeOnlyRange businessHours,Service[] services)
        {
            var location = new Location(locationId,"LocationA",Guid.NewGuid(),3.4,3.4,boxCount,businessHours);
            location.Id = locationId;
            
            location.Services.AddRange(services);
            
            return location;
        }

        public static Location GetDefaultLocation(Guid locationId,Guid ownerId)
        {
            var location = new Location(locationId,"LocationA",ownerId,0.4,0.4,1,new TimeOnlyRange("09:00","18:00"));
            return location;
        }


    }
}
