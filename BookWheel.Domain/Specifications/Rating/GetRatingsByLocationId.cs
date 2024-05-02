using BookWheel.Domain.RatingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Specifications.Rating
{
    public class GetRatingsByLocationId
        :Specification<RatingRoot>
    {
        public GetRatingsByLocationId(Guid LocationId)
            :base(c=>c.LocationId == LocationId)
        {            
        }
    }
}
