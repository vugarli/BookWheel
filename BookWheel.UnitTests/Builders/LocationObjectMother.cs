using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.UnitTests.Builders
{
    public class LocationObjectMother
    {







        public Location CreateLocationWithReservations(int boxCount, params TimeRange[] reservationTimeRanges)
        {
            var location = new Location("LocationA",Guid.NewGuid(),3.4,3.4,boxCount,new TimeOnlyRange("09:00","20:00"));

            foreach(TimeRange timerange in reservationTimeRanges) 
            {
                location.AddReservation(Guid.NewGuid(), timerange);
            }

            return location;
        }



    }
}
