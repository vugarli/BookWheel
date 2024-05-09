using System.Linq.Expressions;
using Location = BookWheel.Domain.LocationAggregate;

namespace BookWheel.Domain.Specifications.Location;

public class GetLocationByOwnerSpecification : Specification<LocationAggregate.Location>
{
    public GetLocationByOwnerSpecification(Guid ownerId) 
        : base(l=>l.OwnerId == ownerId)
    {
        //AddInclude(l=>l.ActiveReservations);
        //AddInclude(l=>l.Services);
    }
}