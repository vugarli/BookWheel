namespace BookWheel.Domain.Specifications.Location
{
    public class GetLocationByIdSpecification
        : Specification<Domain.LocationAggregate.Location>
    {
        public GetLocationByIdSpecification(Guid Id)
            :base(l=>l.Id == Id)
        {
            AddInclude(l=>l.Reservations);
            AddInclude(l=>l.Coordinates);
            AddInclude(l=>l.Services);
        }
    }
}
