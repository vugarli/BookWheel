using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Value_Objects;

namespace BookWheel.UnitTests.Builders;

public class ReservationBuilder
{
    private Guid Id { get; set; } = Guid.NewGuid();
    
    private Guid UserId { get; init; }
    private Guid LocationId { get; init; }
    private List<Service> Services { get; init; }
    private TimeRange ReservationTimeInterval { get; init; }
    
    private int BoxNumber { get; set; } = 1;
    
    public ReservationBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public ReservationBuilder WithBoxNumber(int boxNumber)
    {
        BoxNumber = boxNumber;
        return this;
    }
    
    public ReservationBuilder
        (
            Guid userId,
            Guid locationId,
            List<Service> services,
            TimeRange reservationTimeInterval
        )
    {
        UserId = userId;
        LocationId = locationId;
        Services = services;
        ReservationTimeInterval = reservationTimeInterval;
    }

    public Reservation Build()
    {
        var reservation = 
            new Reservation(
                Id,
                UserId,
                ReservationTimeInterval,
                LocationId,
                BoxNumber,
                Services);

        return reservation;
    }


}