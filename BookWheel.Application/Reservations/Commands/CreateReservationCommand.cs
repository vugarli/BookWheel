using BookWheel.Application.Services;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using BookWheel.Domain;
using FluentValidation;
using MediatR;
using HybridModelBinding;

namespace BookWheel.Application.Reservations.Commands
{
    public class CreateReservationCommand : IRequest
    {
        [HybridBindProperty(Source.Route, order: 10)]
        [Obsolete]
        public Guid LocationId { get; set; }
        
        [HybridBindProperty(Source.Body, order: 10)]
        public DateTimeOffset StartDate { get; set; }

        [HybridBindProperty(Source.Body, order: 10)]
        public List<Guid> ServiceIds { get; set; } = new(); // guard against empty services in domain
    }
    public class CreateReservationCommandValidator
        : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(c => c.LocationId).NotEmpty().NotNull().WithMessage("LocationId must be provided!");
            RuleFor(c => c.StartDate).NotEmpty().NotNull().WithMessage("StartDate must be provided!");
            RuleFor(c => c.ServiceIds).NotEmpty().WithMessage("ServiceIds must be provided!");
            RuleFor(c => c.ServiceIds).Must(c=>c.Distinct().Count() == c.Count()).WithMessage("No duplicate service allowed");
        }
    }
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

            location.AddReservation(userId, services, request.StartDate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

}
