using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Location = BookWheel.Domain.LocationAggregate;

namespace BookWheel.Domain.Specifications.Location
{
    public class GetOwnerLocationByIdSpecification
        : Specification<BookWheel.Domain.LocationAggregate.Location>
    {
        public GetOwnerLocationByIdSpecification(Guid Id,Guid OwnerId) 
            : base(l=>l.OwnerId == OwnerId && l.Id == Id)
        {
        }
    }
}
