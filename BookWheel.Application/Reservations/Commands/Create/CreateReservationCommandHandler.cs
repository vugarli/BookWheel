using BookWheel.Application.Services;
using BookWheel.Domain;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using MediatR;

namespace BookWheel.Application.Reservations.Commands.Create
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

            var services = location.Services.Where(s => request.ServiceIds.Contains(s.Id)).ToList();

            if (services.Count() != request.ServiceIds.Count())
                throw new Exception("Service not found!");
            
            var userIdStr = _currentUserService.GetCurrentUserId();
            Guid.TryParse(userIdStr, out Guid userId);
            
            location.AddReservation(userId,services,request.StartDate);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
