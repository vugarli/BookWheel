namespace BookWheel.Domain.Specifications.Location
{
    public class GetLocationByIdSpecification
        : Specification<Domain.LocationAggregate.Location>
    {
        public GetLocationByIdSpecification(Guid Id)
            :base(l=>l.Id == Id)
        {
            //AddInclude(l=>l.ActiveReservations);
            //AddInclude("ActiveReservations.Services");
            //AddInclude(l=>l.Services);
        }
    }
}
