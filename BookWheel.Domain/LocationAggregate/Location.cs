using BookWheel.Domain.Entities;
using NetTopologySuite.Geometries;
using Guard = Ardalis.GuardClauses.Guard;
using Ardalis.GuardClauses;
using BookWheel.Domain.LocationAggregate.Extensions;
using BookWheel.Domain.Exceptions;
using BookWheel.Domain.Value_Objects;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        public List<Reservation> ActiveReservations { get; init; } = new();

        [NotMapped]
        public List<TimeOnly> TimeSlots { get; set; } = new();


        public Guid ConcurrencyToken { get; private set; }

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


        public List<TimeOnly> GetTimeSlots()
        {
            var groupedTimeTables = new Dictionary<int, List<TimeOnly>>();
            
            for(int b=0;b<BoxCount;b++)
            {
                groupedTimeTables[b] = WorkingTimeRange.GetHours();
                var reservations = ActiveReservations.Where(r=>r.BoxNumber == b+1).ToList();
                
            for(int i=0;i<reservations.Count();i++)
            {
                var res = reservations[i];
                var startTime = TimeOnly.FromTimeSpan(res.ReservationTimeInterval.Start.TimeOfDay);
                var endTime = TimeOnly.FromTimeSpan(res.ReservationTimeInterval.End.TimeOfDay);

                var timeOnlyRange = new TimeOnlyRange(startTime, endTime);

                while (
                    reservations
                    .Any(r => TimeOnly.FromTimeSpan(r.ReservationTimeInterval.Start.TimeOfDay) == timeOnlyRange.End))
                {
                    var reservation = reservations
                    .FirstOrDefault(r => TimeOnly.FromTimeSpan(r.ReservationTimeInterval.Start.TimeOfDay) == timeOnlyRange.End);

                    timeOnlyRange = timeOnlyRange.WithNewEnd(TimeOnly.FromTimeSpan(reservation.ReservationTimeInterval.End.TimeOfDay));

                    reservations.Remove(reservation);
                }
                    groupedTimeTables[b].RemoveAll(r => timeOnlyRange.DoesContain(r));
                    groupedTimeTables[b].Insert(0, timeOnlyRange.End);
            }

            }

            var reservedTimes = groupedTimeTables
                .Values
                .SelectMany(s => s)
                .GroupBy(c=>c.Hour,a=>a,(baseHour,hours)=>hours.Min()).ToList();

            return reservedTimes;
        }

        
        public Guid AddReservation(
            Guid userId,
            List<Service> services,
            DateTimeOffset startDate
        )
        {
            Guard.Against.DuplicateService(services);
            Guard.Against.ServiceDoesNotExist(this,services);
            //TODO check date part is not in range
            
            var durationInMinutes = services.Sum(s=>s.MinuteDuration);
            var reservationTimeInterval = new TimeRange(startDate,TimeSpan.FromMinutes(durationInMinutes));
            
            Guard.Against.OutOfBusinessHours(reservationTimeInterval, this);
            Guard.Against.OutOfTimeSlots(reservationTimeInterval, this);
            
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

                ActiveReservations.Add(newReservation);
                ConcurrencyToken = Guid.NewGuid();
                return newReservation.Id;
            }
            else
            {
                Reservation newReservation = new Reservation(Guid.NewGuid(),userId, reservationTimeInterval, Id, 1,services.ToList());
                ActiveReservations.Add(newReservation);
                ConcurrencyToken = Guid.NewGuid();
                return newReservation.Id;
            }

            // event add
        }

        public void CancelReservationOwner(Guid reservationId)
        {
            Guard.Against.Default(reservationId);
            
            var reservation = ActiveReservations.SingleOrDefault(r=>r.Id == reservationId);

            if (reservation is not null)
            {
                reservation.OwnerCancelReservation();
                // event
            }
        }
        
        public void CancelReservationCustomer(Guid reservationId)
        {
            Guard.Against.Default(reservationId);

            var reservation = ActiveReservations.SingleOrDefault(r=>r.Id == reservationId);

            if (reservation is not null)
            {
                reservation.CustomerCancelReservation();
                // event
            }

        }

        public IEnumerable<Reservation> GetActiveReservations()
        {
            return ActiveReservations.Where(r=>r.IsActive());
        }
        
        public bool DoesOverlapsReservation(Reservation reservation)
        {
            return ActiveReservations.Any(r => r.BoxNumber == reservation.BoxNumber && r.ReservationTimeInterval.DoesOverlap(reservation.ReservationTimeInterval));
        }

        public List<Reservation> GetOverlappingReservations(TimeRange reservationInterval)
        { 
            return ActiveReservations
                .Where(r=>r.IsActive())
                .Where(r => r.ReservationTimeInterval.DoesOverlap(reservationInterval))
                .ToList();
        }

    }
}
