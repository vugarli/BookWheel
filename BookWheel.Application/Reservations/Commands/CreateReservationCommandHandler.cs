using BookWheel.Application.Services;
using BookWheel.Application.Specifications.Locations;
using BookWheel.Domain;
using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Reservations.Commands
{
    public class CreateReservationCommandHandler
        : IRequestHandler<CreateReservationCommand>
    {
        public ILocationRepository _locationRepository { get; }
        public ICurrentUserService _currentUserService { get; }
        public IUnitOfWork _unitOfWork { get; }

        public CreateReservationCommandHandler
            (
            ILocationRepository locationRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork
            )
        {
            _locationRepository = locationRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }


        public async Task Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetLocationByIdSpecification(request.LocationId);
            var location = await _locationRepository.GetLocationBySpecificationAsync(spec);

            if (location is null)
                throw new Exception("Location is not found");

            var userId = _currentUserService.GetCurrentUserId();
            Guid.TryParse(userId, out Guid userIdG);

            var reservation = new Reservation(userIdG,request.ScheduleId,request.LocationId);
            location.AddReservation(reservation);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
