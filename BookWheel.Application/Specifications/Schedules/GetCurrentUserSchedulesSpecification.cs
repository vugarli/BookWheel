using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Specifications.Schedules
{
    public class GetCurrentUserSchedulesSpecification
        : Specification<Schedule>

    {
        public GetCurrentUserSchedulesSpecification(Guid OwnerId)
            : base(s=>s.LocationId == OwnerId)
        {

        }
    }
}
