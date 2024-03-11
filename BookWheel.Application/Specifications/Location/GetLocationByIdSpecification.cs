using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Specifications.Locations
{
    public class GetLocationByIdSpecification
        : Specification<Location>
    {
        public GetLocationByIdSpecification(Guid Id)
            :base(l=>l.Id == Id)
        {
            AddInclude(l=>l.Reservations);
            AddInclude(l=>l.Schedules);
            AddInclude(l=>l.Coordinates);
        }
    }
}
