using BookWheel.Domain.Entities;
using NetTopologySuite.Geometries;
using Guard = Ardalis.GuardClauses.Guard;
using Ardalis.GuardClauses;
using BookWheel.Domain.LocationAggregate.Extensions;
using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Value_Objects;

namespace BookWheel.Domain.LocationAggregate
{
    public class Location : BaseEntity<Guid>, IAggregateRoot
    {
        private Location() {}
        public string Name { get; private set; }
        public Guid OwnerId { get; private set; }
        public Point Coordinates { get; private set; }
        
        public int BoxCount { get; set; } = 1;
        public TimeOnlyRange WorkingTimeRange { get; private set; }
        public List<Service> Services { get; init; } = new();
        
        public List<Reservation> Reservations { get; init; } = new();
        public byte[] Version { get; private set; }

        public Location
            (
            Guid id,
            string name,
            Guid ownerId,
            double latCoord,
            double longCoord,
            int boxCount,
            TimeOnlyRange workingTimeRange
            )
        {
            Guard.Against.InvalidCoordinates(latCoord,longCoord);
            
            Id = Guard.Against.Default(id);
            Name = Guard.Against.NullOrEmpty(name);
            OwnerId = Guard.Against.Default(ownerId);
            
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            Coordinates = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(latCoord, longCoord));
            
            BoxCount = Guard.Against.NegativeOrZero(boxCount);
            WorkingTimeRange = Guard.Against.Null(workingTimeRange);
        }
        
        public void AddService(Service newService)
        {
            if (Services.Any(s => s.Id == newService.Id))
                throw new DuplicateServiceException();
            
            Services.Add(newService);
        }

        public void RemoveService(Guid serviceToDeleteId)
        {
            Guard.Against.InUseService(this,serviceToDeleteId);

            var service = Services.FirstOrDefault(s=>s.Id == serviceToDeleteId);

            if (service is null)
                throw new ServiceNotFoundException(serviceToDeleteId);

            Services.Remove(service);
        }
        
        public void AddReservation(
            Guid userId,
            List<Service> services,
            DateTimeOffset startDate
        )
        {
            Guard.Against.DuplicateService(services);
            Guard.Against.ServiceDoesNotExist(this,services);
            
            var durationInMinutes = services.Sum(s=>s.MinuteDuration);
            var reservationTimeInterval = new TimeRange(startDate,TimeSpan.FromMinutes(durationInMinutes));
            
            Guard.Against.OutOfBusinessHours(reservationTimeInterval, this);
            
            var overlappingReservations = GetOverlappingReservations(reservationTimeInterval);

            if (overlappingReservations.Count() != 0)
            {
                if (overlappingReservations.Select(r => r.BoxNumber).Distinct().Count() == BoxCount)
                    throw new ReservationOverlapsException();

                var newBoxNumber = overlappingReservations
                    .Select(r => r.BoxNumber)
                    .Count() + 1;

                Reservation newReservation = 
                    new Reservation
                    (
                        Guid.NewGuid(),
                        userId,
                        reservationTimeInterval,
                        Id,
                        newBoxNumber,
                        services.ToList()
                    );

                Reservations.Add(newReservation);
            }
            else
            {
                Reservation newReservation = new Reservation(Guid.NewGuid(),userId, reservationTimeInterval, Id, 1,services.ToList());
                Reservations.Add(newReservation);
            }

            // event add
        }

        public void CancelReservationOwner(Guid reservationId)
        {
            Guard.Against.Default(reservationId);
            
            var reservation = Reservations.SingleOrDefault(r=>r.Id == reservationId);

            if (reservation is not null)
            {
                reservation.OwnerCancelReservation();
                // event
            }
        }
        
        public void CancelReservationCustomer(Guid reservationId)
        {
            Guard.Against.Default(reservationId);

            var reservation = Reservations.SingleOrDefault(r=>r.Id == reservationId);

            if (reservation is not null)
            {
                reservation.CustomerCancelReservation();
                // event
            }

        }

        public IEnumerable<Reservation> GetActiveReservations()
        {
            return Reservations.Where(r=>r.IsActive());
        }
        
        public bool DoesOverlapsReservation(Reservation reservation)
        {
            return Reservations.Any(r => r.BoxNumber == reservation.BoxNumber && r.ReservationTimeInterval.DoesOverlap(reservation.ReservationTimeInterval));
        }

        public List<Reservation> GetOverlappingReservations(TimeRange reservationInterval)
        { 
            return Reservations
                .Where(r=>r.IsActive())
                .Where(r => r.ReservationTimeInterval.DoesOverlap(reservationInterval))
                .ToList();
        }

    }
}
